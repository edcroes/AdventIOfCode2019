using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Day09
{
    public class Day09 : IMDay
    {
        private readonly long[] _program;
        private readonly Queue<long> _computerInput = new();
        private readonly List<long> _computerOutput = new();
        private readonly IntCodeComputer _computer;

        public Day09()
        {
            var instructions = InstructionSet.CreateDefaultInstructionSet(() => _computerInput.Dequeue(), i => _computerOutput.Add(i));
            var memSize = 1024 * 32 / sizeof(long); // 1MB 
            _computer = new IntCodeComputer(instructions, memSize);
            _program = IntCodeComputer.ParseProgram(File.ReadAllText("Day09\\input.txt"));
        }

        public string GetAnswerPart1()
        {
            _computerOutput.Clear();
            _computerInput.Enqueue(1);
            _computer.Execute(_program);
            return _computerOutput.Last().ToString();
        }

        public string GetAnswerPart2()
        {
            _computerOutput.Clear();
            _computerInput.Enqueue(2);
            _computer.Execute(_program);
            return _computerOutput.Last().ToString();
        }
    }
}
