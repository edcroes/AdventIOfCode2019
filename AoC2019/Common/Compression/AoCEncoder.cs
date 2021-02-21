using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Common.Compression
{
    public class AoCEncoder
    {
        private readonly int _numberOfParts;
        private readonly int _maxPartLength;

        public AoCEncoder(int numberOfParts, int maxPartLength)
        {
            if (numberOfParts < 2)
            {
                throw new ArgumentException("You must have at least 2 parts for compression");
            }

            _numberOfParts = numberOfParts;
            _maxPartLength = maxPartLength;
        }

        public AoCArchive<string> Encode(IEnumerable<string> inputToCompress)
        {
            var input = inputToCompress.ToArray();
            var options = GetOptions(input);

            foreach (var option in options)
            {
                EliminateInvalidNextParts(option);
            }

            var validOption = options.FirstOrDefault(o => o.NextParts.Any());

            if (validOption == null)
            {
                throw new InvalidOperationException("Input cannot be compressed with the options given");
            }

            var parts = new List<string[]> { validOption.Contents };
            var deepestPart = validOption;
            while (deepestPart.NextParts.Any())
            {
                deepestPart = deepestPart.NextParts.First();
                parts.Add(deepestPart.Contents);
            }

            List<int> constructed = new();
            foreach (var part in deepestPart.CompressedInput)
            {
                constructed.Add(int.Parse(part.Replace("-", "").Replace("|", "")));
            }

            return new AoCArchive<string>
            {
                Constructed = constructed.ToArray(),
                Parts = parts.ToArray()
            };
        }

        private List<Part> GetOptions(string[] input)
        {
            var parts = new List<Part>();
            var maxPartLength = Math.Min(_maxPartLength, input.Length - _numberOfParts);

            for (var length = 1; length <= maxPartLength; length++)
            {
                var part = new Part(0, input.Take(length).ToArray(), input);
                FindNextOptions(part);
                parts.Add(part);
            }

            return parts;
        }

        private void FindNextOptions(Part part)
        {
            var remainingParts = GetRemainingParts(part);

            if (remainingParts.Length == 0)
            {
                return;
            }

            var maxPartLength = Math.Min(_maxPartLength, remainingParts[0].Length);

            for (var length = 1; length <= maxPartLength; length++)
            {
                var newPart = new Part(part.Depth + 1, remainingParts[0].Take(length).ToArray(), part.CompressedInput, part);

                if (newPart.Depth + 1 < _numberOfParts)
                {
                    FindNextOptions(newPart);
                }

                part.NextParts.Add(newPart);
            }
        }

        private void EliminateInvalidNextParts(Part part)
        {
            foreach (var nextPart in part.NextParts)
            {
                EliminateInvalidNextParts(nextPart);
            }

            if (part.Depth < _numberOfParts - 1 && GetRemainingParts(part).Length == 0)
            {
                part.NextParts.Clear();
            }
            else if (part.Depth + 1 == _numberOfParts - 1)
            {
                var invalidParts = part.NextParts.Where(p => p.NextParts.Count > 0 || GetRemainingParts(p).Length > 0).ToArray();
                foreach (var invalidPart in invalidParts)
                {
                    part.NextParts.Remove(invalidPart);
                }
            }
            else
            {
                var invalidParts = part.NextParts.Where(p => p.NextParts.Count == 0).ToArray();
                foreach (var invalidPart in invalidParts)
                {
                    part.NextParts.Remove(invalidPart);
                }
            }
        }

        private static string[][] GetRemainingParts(Part part)
        {
            var splits = new List<string> { part.Replacement };
            var parent = part;
            while ((parent = parent.Parent) != null)
            {
                splits.Add(parent.Replacement);
            }

            var remainingParts = part.CompressedInput.Split(splits.ToArray());
            return remainingParts;
        }

        private class Part
        {
            public Part(int depth, string[] contents, string[] input, Part parent = null)
            {
                Depth = depth;
                Contents = contents;
                Parent = parent;
                CompressedInput = GenerateCompressedInput(contents, input);
            }

            public int Depth { get; init; }

            public string Replacement => $"-|{Depth}|-";

            public string[] Contents { get; init; }

            public string[] CompressedInput { get; init; }

            public Part Parent { get; init; }

            public List<Part> NextParts { get; } = new();

            private string[] GenerateCompressedInput(string[] contents, string[] input)
            {
                return input.Replace(contents, new[] { Replacement });
            }
        }
    }
}
