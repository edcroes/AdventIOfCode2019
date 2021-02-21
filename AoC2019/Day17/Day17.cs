using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2019.Common;
using AoC2019.Common.Compression;
using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;
using AoC2019.Common.Maps;

namespace AoC2019.Day17
{
    public class Day17 : IMDay
    {
        private readonly long[] _program;
        private BlockingCollection<long> _input;
        private List<long> _output;

        private static readonly Dictionary<char, Point> _validVacuumPositions = new()
        {
            { '^', new Point(0, -1) },
            { '>', new Point(1, 0) },
            { 'v', new Point(0, 1) },
            { '<', new Point(-1, 0) },
        };

        public Day17()
        {
            _program = IntCodeComputer.ParseProgram(File.ReadAllText("Day17\\input.txt"));
        }

        public string GetAnswerPart1()
        {
            _output = new List<long>();

            StartComputer(() => 0L);

            var map = ConstructMap(_output);
            var intersections = GetIntersections(map);

            return intersections.Sum(i => i.X * i.Y).ToString();
        }

        public string GetAnswerPart2()
        {
            _output = new List<long>();
            _program[0] = 2;

            var computerTask = Task.Run(() => StartComputer(GetInput));
            computerTask.Wait();

            return _output.Last().ToString();
        }

        private static Map<char> ConstructMap(List<long> output)
        {
            var allLines = new List<char[]>();
            var currentLine = string.Empty;

            foreach (var c in output)
            {
                var character = (char)c;
                if (character == '\n')
                {
                    if (allLines.Count == 0 || allLines[0].Length == currentLine.Length)
                    {
                        allLines.Add(currentLine.ToCharArray());
                    }
                    currentLine = string.Empty;
                }
                else
                {
                    currentLine += character;
                }
            }

            return new Map<char>(allLines.Where(l => l.Length > 0).ToArray());
        }

        private static List<Point> GetIntersections(Map<char> map)
        {
            var intersections = new List<Point>();

            for (var y = 1; y < map.SizeY - 1; y++)
            {
                for (var x = 1; x < map.SizeX - 1; x++)
                {
                    var point = new Point(x, y);
                    var value = map.GetValue(x, y);
                    if (value == '#' && map.NumberOfStraightNeighborsThatMatch(point, '#') == 4)
                    {
                        intersections.Add(point);
                    }
                }
            }

            return intersections;
        }

        private long GetInput()
        {
            if (_input == null)
            {
                InitializeInput();
            }

            return _input.Take();
        }

        private void InitializeInput()
        {
            _input = new BlockingCollection<long>();
            var map = ConstructMap(_output);
            _output.Clear();

            var directions = GetVacuumDirections(map);
            var compressedDirections = new AoCEncoder(3, 10).Encode(directions);

            var instructions = string.Join(",", compressedDirections.Constructed.Select(i => (char)(i + 65))) + "\n";
            foreach (var part in compressedDirections.Parts)
            {
                instructions += string.Join(",", part) + "\n";
            }

            foreach (var character in instructions.ToCharArray())
            {
                _input.Add(character);
            }
            _input.Add('n'); // Camera feed: y = on, n = off
            _input.Add('\n');
        }

        private static string[] GetVacuumDirections(Map<char> map)
        {
            List<string> directions = new();
            var currentPosition = map.First((p, v) => _validVacuumPositions.Keys.Contains(v));
            var direction = _validVacuumPositions[map.GetValue(currentPosition)];

            while (true)
            {
                var left = _validVacuumPositions.Values.GetPreviousItem(direction);
                var right = _validVacuumPositions.Values.GetNextItem(direction);

                bool? turnLeft = map.GetValue(currentPosition.Add(left)) == '#' ? true : map.GetValue(currentPosition.Add(right)) == '#' ? false : null;

                if (!turnLeft.HasValue)
                {
                    break;
                }

                var nextPosition = GetNextPosition(map, currentPosition, turnLeft.Value ? left : right);
                var movement = Math.Abs((currentPosition.X - nextPosition.X) + (currentPosition.Y - nextPosition.Y));
                currentPosition = nextPosition;
                direction = turnLeft.Value ? left : right;

                directions.Add(turnLeft.Value ? "L" : "R");
                directions.Add(movement.ToString());
            }

            return directions.ToArray();
        }

        private static Point GetNextPosition(Map<char> map, Point currentPosition, Point movement)
        {
            var nextPosition = currentPosition;

            while (map.GetValueOrDefault(nextPosition.Add(movement)) == '#')
            {
                nextPosition = nextPosition.Add(movement);
            }

            return nextPosition;
        }

        private void StartComputer(Func<long> getInput)
        {
            var instructionsSet = InstructionSet.CreateDefaultInstructionSet(getInput, l => _output.Add(l));
            var computer = new IntCodeComputer(instructionsSet, 8192);
            computer.Execute(_program);
        }
    }
}
