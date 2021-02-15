using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;
using AoC2019.Common.Maps;

namespace AoC2019.Day15
{
    public class Day15 : IMDay
    {
        private readonly long[] _program;
        private BlockingCollection<long> _input;
        private BlockingCollection<MoveResult> _output;

        private readonly static Point3D StartingPoint = new Point3D(0, 0, 0);
        private Point3D _currentPosition;
        private Point3D _oxygenSystemPosition;
        private List<Point3D> _knownPoints;
        private List<Point> _walls;

        public Day15()
        {
            _program = IntCodeComputer.ParseProgram(File.ReadAllText("Day15\\input.txt"));
        }

        public string GetAnswerPart1()
        {
            StartComputer();

            var currentLength = 0;
            while (_oxygenSystemPosition == StartingPoint)
            {
                DiscoverKnownPoints(currentLength);
                currentLength++;
            }

            return _oxygenSystemPosition.Z.ToString();
        }

        public string GetAnswerPart2()
        {
            StartComputer();

            var currentLength = 0;
            while (_knownPoints.Any(p => p.Z == currentLength))
            {
                DiscoverKnownPoints(currentLength);
                currentLength++;
            }

            var partsWithOxygen = new HashSet<Point3D> { _oxygenSystemPosition };
            var currentOxygenPoints = new List<Point3D> { _oxygenSystemPosition };
            var minute = 0;

            while (partsWithOxygen.Count < _knownPoints.Count)
            {
                minute++;
                currentOxygenPoints = currentOxygenPoints.SelectMany(p => _knownPoints.Where(kp => !partsWithOxygen.Contains(kp) && IsAdjecent(p, kp))).ToList();

                foreach (var point in currentOxygenPoints)
                {
                    partsWithOxygen.Add(point);
                }
            }

            return minute.ToString();
        }

        private void DiscoverKnownPoints(int length)
        {
            var pointsToExplore = _knownPoints.Where(p => p.Z == length).ToArray();
            foreach (var point in pointsToExplore)
            {
                var newPointsToDiscover = new[]
                {
                    new Point3D(point.X, point.Y - 1, length + 1),
                    new Point3D(point.X, point.Y + 1, length + 1),
                    new Point3D(point.X - 1, point.Y, length + 1),
                    new Point3D(point.X + 1, point.Y, length + 1)
                };

                MoveTo(point);

                foreach (var newPoint in newPointsToDiscover)
                {
                    var result = DiscoverNewPoint(newPoint);
                    if (result == MoveResult.OxygenSystem)
                    {
                        _oxygenSystemPosition = newPoint;
                    }
                }
            }
        }

        private MoveResult DiscoverNewPoint(Point3D point)
        {
            var point2d = new Point(point.X, point.Y);
            if (_walls.Contains(point2d))
            {
                return MoveResult.Wall;
            }

            if (_knownPoints.Any(p => p.X == point.X && p.Y == point.Y))
            {
                return MoveResult.Moved;
            }

            var currentPosition = _currentPosition;
            var result = MoveToAdjecentPoint(point);
            if (result == MoveResult.Wall)
            {
                _walls.Add(point2d);
                return result;
            }

            _knownPoints.Add(point);
            MoveToAdjecentPoint(currentPosition);

            return result;
        }

        private void MoveTo(Point3D point)
        {
            if (point == _currentPosition)
            {
                return;
            }

            if (IsAdjecent(_currentPosition, point))
            {
                MoveToAdjecentPoint(point);
                return;
            }

            var pathToPoint = GetKnownPathToStartingPoint(point);
            var pathToStart = GetKnownPathToStartingPoint(_currentPosition);

            while (!pathToPoint.Contains(_currentPosition))
            {
                var nextIndex = pathToStart.IndexOf(_currentPosition) + 1;
                MoveToAdjecentPoint(pathToStart[nextIndex]);
            }

            while (_currentPosition != point)
            {
                var nextIndex = pathToPoint.IndexOf(_currentPosition) - 1;
                MoveToAdjecentPoint(pathToPoint[nextIndex]);
            }
        }

        private MoveResult MoveToAdjecentPoint(Point3D point)
        {
            var direction = GetMoveDirection(_currentPosition, point);
            _input.Add((long)direction);
            var moveResult = _output.Take();

            if (moveResult != MoveResult.Wall)
            {
                _currentPosition = point;
            }

            return moveResult;
        }

        private List<Point3D> GetKnownPathToStartingPoint(Point3D end)
        {
            var path = new List<Point3D> { end };
            var current = end;

            while (current != StartingPoint)
            {
                current = _knownPoints.First(p => p.Z == current.Z - 1 && IsAdjecent(p, current));
                path.Add(current);
            }

            return path;
        }

        private static Direction GetMoveDirection(Point3D from, Point3D to)
        {
            var xMovement = to.X - from.X;
            var yMovement = to.Y - from.Y;

            return (xMovement, yMovement) switch
            {
                (0, -1) => Direction.North,
                (0, 1) => Direction.South,
                (-1, 0) => Direction.West,
                (1, 0) => Direction.East,
                _ => throw new InvalidOperationException($"Unable to move from {from} to {to}")
            };
        }

        private static bool IsAdjecent(Point3D first, Point3D second) =>
                (Math.Abs(first.X - second.X) == 1 && first.Y == second.Y) ||
                (Math.Abs(first.Y - second.Y) == 1 && first.X == second.X);

        private void StartComputer()
        {
            Reset();

            var instructionsSet = InstructionSet.CreateDefaultInstructionSet(() => _input.Take(), l => _output.Add((MoveResult)l));
            var computer = new IntCodeComputer(instructionsSet);
            Task.Run(() => computer.Execute(_program));
        }

        private void Reset()
        {
            _input = new BlockingCollection<long>();
            _output = new BlockingCollection<MoveResult>();
            _currentPosition = StartingPoint;
            _oxygenSystemPosition = StartingPoint;
            _knownPoints = new List<Point3D> { _currentPosition };
            _walls = new List<Point>();
        }

        private enum Direction
        {
            North = 1,
            South = 2,
            West = 3,
            East = 4
        }

        private enum MoveResult
        {
            Wall = 0,
            Moved = 1,
            OxygenSystem = 2,
            AlreadyThere
        }
    }
}
