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

namespace AoC2019.Day13
{
    public class Day13 : IMDay
    {
        private readonly bool _displayGame;
        private const int GameSpeedMilliseconds = 20;

        private const int Paddle = 3;
        private const int Ball = 4;
        private readonly Point ScoreLocation = new Point(-1, 0);

        private long _score = 0;
        private GameInstruction _currentInstruction;
        private Point _paddleLocation;
        private BlockingCollection<long> _input;

        private readonly char[] OutputGraphics = new[]
        {
            ' ',
            '█',
            '░',
            '=',
            'o'
        };

        public Day13() : this(false) { }

        public Day13(bool displayGame)
        {
            _displayGame = displayGame;
        }

        public string GetAnswerPart1()
        {
            List<int> output = new();
            var computer = CreateComputer(() => 0, l => output.Add((int)l));
            var program = IntCodeComputer.ParseProgram(File.ReadAllText("Day13\\input.txt"));

            computer.Execute(program);
            var endGame = ParseEndGame(output);

            return endGame.Values.Count(v => v == 2).ToString();
        }

        public string GetAnswerPart2()
        {
            Prepare();

            var computer = CreateComputer(() => _input.Take(), ProcessOutput);
            var program = IntCodeComputer.ParseProgram(File.ReadAllText("Day13\\input.txt"));
            program[0] = 2;

            var computerTask = Task.Run(() => computer.Execute(program));
            computerTask.Wait();

            return _score.ToString();
        }

        private void ProcessOutput(long output)
        {
            _currentInstruction.Add(output);

            if (_currentInstruction.IsComplete)
            {
                ProcessInstruction(_currentInstruction);
                _currentInstruction = new GameInstruction();
            }
        }

        private void ProcessInstruction(GameInstruction instruction)
        {
            DrawInstruction(instruction);

            if (instruction.Location == ScoreLocation)
            {
                _score = instruction.Value;
                DrawScore();
            }
            else if (instruction.Value == Paddle)
            {
                _paddleLocation = instruction.Location;
            }
            else if (instruction.Value == Ball)
            {
                var x = CalculatePaddleNextX(instruction.Location);
                _input.Add(x);
            }
        }

        private void DrawInstruction(GameInstruction instruction)
        {
            if (_displayGame && instruction.Location != ScoreLocation)
            {
                Console.SetCursorPosition(instruction.Location.X, instruction.Location.Y);
                Console.Write(OutputGraphics[instruction.Value]);

                if (instruction.Value == Ball)
                {
                    Thread.Sleep(GameSpeedMilliseconds);
                }
            }
        }

        private int CalculatePaddleNextX(Point ballLocation) => ballLocation.X.CompareTo(_paddleLocation.X);

        private void DrawScore()
        {
            if (_displayGame)
            {
                Console.SetCursorPosition(0, 22);
                Console.WriteLine($"Score: {_score}");
            }
        }

        private void Prepare()
        {
            _input = new BlockingCollection<long>();
            _score = 0;
            _currentInstruction = new GameInstruction();
        }

        private static Dictionary<Point, int> ParseEndGame(List<int> output)
        {
            Dictionary<Point, int> result = new();

            for (var i = 0; i < output.Count; i += 3)
            {
                var point = new Point(output[i], output[i + 1]);

                if (result.ContainsKey(point))
                {
                    result[point] = output[i + 2];
                }
                else
                {
                    result.Add(point, output[i + 2]);
                }
            }

            return result;
        }

        private static IntCodeComputer CreateComputer(Func<long> getInput, Action<long> setOutput)
        {
            var instructions = InstructionSet.CreateDefaultInstructionSet(getInput, setOutput);
            return new IntCodeComputer(instructions, 4096);
        }

        private class GameInstruction
        {
            private long[] _parts = new long[3];
            private byte _currentPosition = 0;

            public bool IsComplete => _currentPosition == 3;
            public Point Location => new Point((int)_parts[0], (int)_parts[1]);
            public long Value => _parts[2];

            public void Add(long value)
            {
                if (_currentPosition > 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "You can only call Add three times");
                }

                _parts[_currentPosition] = value;
                _currentPosition++;
            }
        }
    }
}
