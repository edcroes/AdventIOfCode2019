using AoC2019.Common.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AoC2019.Day03
{
    public class Day03 : IMDay
    {
        private readonly Movement[][] _wires;

        public Day03()
        {
            _wires = File.ReadAllLines("Day03\\input.txt")
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(m => Movement.Parse(m))
                    .ToArray())
                .ToArray();
        }

        public string GetAnswerPart1()
        {
            return CalculateManhattenDistanceByKeepingTrackOfLines().ToString();
        }

        public string GetAnswerPart2()
        {
            return CalculateStepsByKeepingTrackOfEachPosition().ToString();
        }

        private int CalculateManhattenDistanceByKeepingTrackOfLines()
        {
            List<Line> allWireLines = new();
            HashSet<Point> _crossingPoints = new();

            foreach (var wire in _wires)
            {
                var x = 0;
                var y = 0;
                List<Line> _wireLines = new();

                foreach (var movement in wire)
                {
                    var toX = x + GetMovementX(movement) * movement.Distance;
                    var toY = y + GetMovementY(movement) * movement.Distance;
                    var line = new Line(new Point(x, y), new Point(toX, toY));
                    _wireLines.Add(line);
                    x = toX;
                    y = toY;

                    var crossedPoints = allWireLines
                        .Select(l => l.GetIntersectionWithLineSegment(line))
                        .Where(p => p.HasValue)
                        .Select(p => Point.Truncate(p.Value))
                        .Distinct()
                        .Where(p => !_crossingPoints.Contains(p));

                    foreach (var crossedPoint in crossedPoints)
                    {
                        _crossingPoints.Add(crossedPoint);
                    }
                }

                allWireLines.AddRange(_wireLines);
            }

            var manhattenDistanceOfClosestPointToZero = _crossingPoints
                .Select(p => Math.Abs(p.X) + Math.Abs(p.Y))
                .Min();

            return manhattenDistanceOfClosestPointToZero;
        }

        private int CalculateStepsByKeepingTrackOfEachPosition()
        {
            List<Dictionary<Point, int>> allWirePaths = new();
            Dictionary<Point, int> _crossingPoints = new();

            foreach (var wire in _wires)
            {
                var stepsTaken = 0;
                var x = 0;
                var y = 0;
                Dictionary<Point, int> wirePath = new();

                foreach (var movement in wire)
                {
                    var movementX = GetMovementX(movement);
                    var movementY = GetMovementY(movement);

                    for (var i = 0; i < movement.Distance; i++)
                    {
                        x += movementX;
                        y += movementY;
                        var nextPosition = new Point(x, y);
                        stepsTaken++;

                        if (!wirePath.ContainsKey(nextPosition))
                        {
                            wirePath.Add(nextPosition, stepsTaken);
                        }

                        var lowestStepsTakenToCrossPoint = allWirePaths.Where(l => l.ContainsKey(nextPosition)).Select(l => l[nextPosition]).OrderBy(s => s).FirstOrDefault();
                        if (lowestStepsTakenToCrossPoint > 0)
                        {
                            var totalSteps = stepsTaken + lowestStepsTakenToCrossPoint;
                            if (_crossingPoints.ContainsKey(nextPosition))
                            {
                                if (_crossingPoints[nextPosition] > lowestStepsTakenToCrossPoint)
                                {
                                    _crossingPoints[nextPosition] = lowestStepsTakenToCrossPoint;
                                }
                            }
                            else
                            {
                                _crossingPoints.Add(nextPosition, totalSteps);
                            }
                        }
                    }
                }

                allWirePaths.Add(wirePath);
            }

            var lowestStepsTakenToCrossingPoint = _crossingPoints.Values.Min();
            return lowestStepsTakenToCrossingPoint;
        }

        private static int GetMovementX(Movement m) => m.Direction switch
        {
            'L' => -1,
            'R' => 1,
            _ => 0
        };

        private static int GetMovementY(Movement m) => m.Direction switch
        {
            'U' => -1,
            'D' => 1,
            _ => 0
        };

        private struct Movement
        {
            public char Direction { get; set; }
            public short Distance { get; set; }

            public static Movement Parse(string input)
            {
                return new Movement
                {
                    Direction = input[0],
                    Distance = short.Parse(input[1..])
                };
            }
        }
    }
}
