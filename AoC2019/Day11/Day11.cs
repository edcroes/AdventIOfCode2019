using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using AoC2019.Common;
using System.Linq;

namespace AoC2019.Day11
{
    public class Day11 : IMDay
    {
        private readonly BlockingCollection<long> _input = new();
        private readonly IntCodeComputer _computer;
        private readonly long[] _program;
        private readonly Point[] _orientations = new[]
        {
            new Point(0, -1), // Up
            new Point(1, 0),  // Right
            new Point(0, 1),  // Down
            new Point(-1, 0)  // Left
        };

        private readonly Dictionary<Point, long> _spaces = new();
        private bool _nextOutputIsPaint;
        private Point _currentOrientation;
        private Point _currentPosition;

        public Day11()
        {
            _program = IntCodeComputer.ParseProgram(File.ReadAllText("Day11\\input.txt"));
            var instructions = InstructionSet.CreateDefaultInstructionSet(() => _input.Take(), ProcessOutput);
            _computer = new IntCodeComputer(instructions, 2048);
        }

        public string GetAnswerPart1()
        {
            Reset();
            _input.Add(0);

            var computerTask = Task.Run(() => _computer.Execute(_program));
            Task.WaitAll(computerTask);

            return _spaces.Keys.Count.ToString();
        }

        public string GetAnswerPart2()
        {
            Reset();
            _input.Add(1);

            var computerTask = Task.Run(() => _computer.Execute(_program));
            Task.WaitAll(computerTask);

            return GetDrawing();
        }

        private void ProcessOutput(long l)
        {
            if (_nextOutputIsPaint)
            {
                ApplyPaint(_currentPosition, l);
            }
            else
            {
                _currentOrientation = l == 0
                    ? _orientations.GetPreviousItem(_currentOrientation)
                    : _orientations.GetNextItem(_currentOrientation);
                _currentPosition = new Point(_currentPosition.X + _currentOrientation.X, _currentPosition.Y + _currentOrientation.Y);

                _input.Add(GetColor(_currentPosition));
            }

            _nextOutputIsPaint = !_nextOutputIsPaint;
        }

        private void ApplyPaint(Point position, long color)
        {
            if (_spaces.ContainsKey(position))
            {
                _spaces[position] = color;
            }
            else
            {
                _spaces.Add(position, color);
            }
        }

        private long GetColor(Point position) => _spaces.ContainsKey(position) ? _spaces[position] : 0;

        private void Reset()
        {
            while (_input.Count > 0)
            {
                _input.Take();
            }
            _spaces.Clear();
            _nextOutputIsPaint = true;
            _currentOrientation = _orientations[0];
            _currentPosition = new Point(0, 0);
        }

        private string GetDrawing()
        {
            var minY = _spaces.Keys.Select(p => p.Y).Min();
            var maxY = _spaces.Keys.Select(p => p.Y).Max();
            var minX = _spaces.Keys.Select(p => p.X).Min();
            var maxX = _spaces.Keys.Select(p => p.X).Max();

            var border = string.Empty;
            var emptyLine = string.Empty;
            for (var i = minX; i <= maxX; i++)
            {
                border += "═";
                emptyLine += " ";
            }

            var result = $"{Environment.NewLine}╔{border}╗{Environment.NewLine}";
            for (var y = minY; y <= maxY; y++)
            {
                result += "║";
                for (var x = minX; x <= maxX; x++)
                {
                    var point = new Point(x, y);
                    var color = GetColor(point);
                    result += color == 0 ? " " : "█";
                }
                
                result += $"║{Environment.NewLine}";
            }
            result += $"╚{border}╝";

            return result;
        }
    }
}
