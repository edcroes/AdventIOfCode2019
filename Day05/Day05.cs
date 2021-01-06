using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;

namespace AoC2019.Day05
{
    public class Day05 : IMDay
    {
        private readonly IntCodeComputer _computer;
        private readonly List<int> _computerOutput = new();
        private int _systemToTest;

        public Day05()
        {
            var instructions = InstructionSet.CreateDefaultInstructionSet(() => _systemToTest, i => _computerOutput.Add(i));
            _computer = new IntCodeComputer(instructions);
        }

        public string GetAnswerPart1()
        {
            _systemToTest = 1;
            _computerOutput.Clear();
            var program = ParseProgram(File.ReadAllText("Day05\\input.txt"));
            _computer.Execute(program);

            return _computerOutput.Last().ToString();
        }

        public string GetAnswerPart2()
        {
            _systemToTest = 5;
            _computerOutput.Clear();
            var program = ParseProgram(File.ReadAllText("Day05\\input.txt"));
            _computer.Execute(program);

            return _computerOutput.Last().ToString();
        }

        private static int[] ParseProgram(string program) => program.Split(",").Select(i => int.Parse(i)).ToArray();


    }
}
