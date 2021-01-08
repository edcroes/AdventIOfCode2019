using AoC2019.Common;
using System;
using System.IO;
using System.Linq;

namespace AoC2019.Day08
{
    public class Day08 : IMDay
    {
        private readonly byte[][] _layers;
        private const int ImageWidth = 25;
        private const int ImageSize = ImageWidth * 6;

        public Day08()
        {
            _layers = File.ReadAllText("Day08\\input.txt").Trim()
                .ToCharArray()
                .Select(c => byte.Parse(c.ToString()))
                .Split(ImageSize);
        }

        public string GetAnswerPart1()
        {
            var layerToCheck = _layers.OrderBy(l => l.Count(p => p == 0)).First();
            var answer = layerToCheck.Count(p => p == 1) * layerToCheck.Count(p => p == 2);
            return answer.ToString();
        }

        public string GetAnswerPart2()
        {
            var finalImage = new byte[ImageSize];
            foreach (var layer in _layers)
            {
                for (var i = 0; i < ImageSize; i++)
                {
                    // Yes I'm lazy, in the final image 1 is black and 2 is white
                    if (finalImage[i] == 0 && layer[i] != 2)
                    {
                        finalImage[i] = layer[i] == 0 ? 1 : 2;
                    }
                }
            }

            var border = string.Empty;
            for (var i = 0; i < ImageWidth; i++) border += "═";
            
            var result = $"{Environment.NewLine}╔{border}╗{Environment.NewLine}║";
            for (var i = 0; i < ImageSize; i++)
            {
                if (i % ImageWidth == 0 && i != 0)
                {
                    result += $"║{Environment.NewLine}║";
                }
                result += finalImage[i] == 1 ? ' ' : '█';
            }
            result += $"║{Environment.NewLine}╚{border}╝";

            return result;
        }
    }
}
