using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AoC2019.Common;
using AoC2019.Common.Maps;

namespace AoC2019.Day24
{
    public class Day24 : IMDay
    {
        private readonly string[] _input;

        public Day24()
        {
            _input = File.ReadAllLines("Day24\\input.txt");
        }

        public string GetAnswerPart1()
        {
            var seenRatings = new List<int>();
            var map = ParseInput(_input);

            var currentMinute = 0;
            var lastSeenRating = GetBiodiversityRating(map);
            while (!seenRatings.Contains(lastSeenRating))
            {
                seenRatings.Add(lastSeenRating);
                currentMinute++;
                map.DistributeChaos(true, NumberOfNeighborsThatMatch, GetNewValue);
                lastSeenRating = GetBiodiversityRating(map);
            }

            return lastSeenRating.ToString();
        }

        public string GetAnswerPart2()
        {
            // So we don't want a flat map... we want a 3D map on which z should be something like infinite
            // We need to run it for 200 minutes. Each minute the grid can grow up and down (at least in theory, in this case it can grow every 2 minutes). So we need at least a z of 401.
            var numberOfMinutesToRun = 200;
            var map = new Map3D<bool>(_input[0].Length, _input.Length, numberOfMinutesToRun * 2 + 1);

            var initialMap = ParseInput(_input).To2DArray();
            initialMap[(initialMap.GetLength(0) - 1) / 2, (initialMap.GetLength(0) - 1) / 2] = false;
            map.PlaceInMiddle(initialMap);

            for (var minute = 1; minute <= numberOfMinutesToRun; minute++)
            {
                map.DistributeChaos(true, NumberOfNeighborsThatMatchPart2, GetNewValue);
            }

            return map.Count(true).ToString();
        }

        private static Map<bool> ParseInput(string[] input)
        {
            var map = new Map<bool>(input[0].Length, input.Length);

            for (var y = 0; y < input.Length; y++)
            {
                var chars = input[y].ToCharArray();
                for (var x = 0; x < chars.Length; x++)
                {
                    map.SetValue(x, y, chars[x] == '#');
                }
            }

            return map;
        }

        private static int NumberOfNeighborsThatMatch(Map<bool> map, Point point)
        {
            var numberOfMatches = 0;

            if (map.GetValueOrDefault(point.X - 1, point.Y)) numberOfMatches++;
            if (map.GetValueOrDefault(point.X + 1, point.Y)) numberOfMatches++;
            if (map.GetValueOrDefault(point.X, point.Y - 1)) numberOfMatches++;
            if (map.GetValueOrDefault(point.X, point.Y + 1)) numberOfMatches++;

            return numberOfMatches;
        }

        private static bool GetNewValue(bool isAlive, int numberOfNeighborsThatMatch) => (isAlive && numberOfNeighborsThatMatch == 1) || (!isAlive && numberOfNeighborsThatMatch >= 1 && numberOfNeighborsThatMatch <= 2);

        private static int GetBiodiversityRating(Map<bool> map) => map.ToFlatArray().Reverse().ToArray().ConvertToInt32();

        private static int NumberOfNeighborsThatMatchPart2(Map3D<bool> map, Point3D point)
        {
            var numberOfMatches = 0;
            var centerY = (map.SizeY - 1) / 2;
            var centerX = (map.SizeX - 1) / 2;

            if (point.X == centerX && point.Y == centerY)
            {
                return numberOfMatches;
            }

            var pointsToCheck = new[]
            {
                new Point(point.X - 1, point.Y),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X, point.Y + 1)
            };

            foreach (var neighbor in pointsToCheck)
            {
                if (neighbor.X == centerX && neighbor.Y == centerY)
                {
                    numberOfMatches += GetNumberOfMatchesFromInception(map, point);
                }
                else if (neighbor.X < 0)
                {
                    // Left
                    if (point.Z > 0 && map.GetValueOrDefault(centerX - 1, centerY, point.Z - 1))
                    {
                        numberOfMatches++;
                    }
                }
                else if (neighbor.X >= map.SizeX)
                {
                    // Right
                    if (point.Z > 0 && map.GetValueOrDefault(centerX + 1, centerY, point.Z - 1))
                    {
                        numberOfMatches++;
                    }
                }
                else if (neighbor.Y < 0)
                {
                    // Top
                    if (point.Z > 0 && map.GetValueOrDefault(centerX, centerY - 1, point.Z - 1))
                    {
                        numberOfMatches++;
                    }
                }
                else if (neighbor.Y >= map.SizeY)
                {
                    // Bottom
                    if (point.Z > 0 && map.GetValueOrDefault(centerX, centerY + 1, point.Z - 1))
                    {
                        numberOfMatches++;
                    }
                }
                else if (map.GetValueOrDefault(neighbor.X, neighbor.Y, point.Z))
                {
                    numberOfMatches++;
                }
            }

            return numberOfMatches;
        }

        private static int GetNumberOfMatchesFromInception(Map3D<bool> map, Point3D point)
        {
            var z = point.Z + 1;
            bool[] line = null;

            if (z >= map.SizeZ)
            {
                return 0;
            }

            if (point.X < (map.SizeX - 1) / 2)
            {
                // Left
                line = map.GetLine(0, 0, z, 0, map.SizeY - 1, z);
            }
            else if (point.X > (map.SizeX - 1) / 2)
            {
                // Right
                line = map.GetLine(map.SizeX - 1, 0, z, map.SizeX - 1, map.SizeY - 1, z);
            }
            else if (point.Y < (map.SizeY - 1) / 2)
            {
                // Top
                line = map.GetLine(0, 0, z, map.SizeX - 1, 0, z);
            }
            else if (point.Y > (map.SizeY - 1) / 2)
            {
                // Bottom
                line = map.GetLine(0, map.SizeY - 1, z, map.SizeX - 1, map.SizeY - 1, z);
            }

            return line.Count(v => v);
        }
    }
}
