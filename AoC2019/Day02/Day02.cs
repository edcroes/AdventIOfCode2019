using System.IO;
using System.Linq;
using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;

namespace AoC2019.Day02
{
    public class Day02 : IMDay
    {
        private readonly IntCodeComputer _computer;

        public Day02()
        {
            var instructions = new InstructionSet
            {
                { 1, new Add() },
                { 2, new Multiply() },
                { 99, new Halt() }
            };
            _computer = new IntCodeComputer(instructions);
        }

        public string GetAnswerPart1()
        {
            var program = IntCodeComputer.ParseProgram(File.ReadAllText("Day02\\input.txt"));
            program[1] = 12;
            program[2] = 2;
            _computer.Execute(program);

            var answer = _computer.GetMemory(0);
            return answer.ToString();
        }

        public string GetAnswerPart2()
        {
            var program = IntCodeComputer.ParseProgram(File.ReadAllText("Day02\\input.txt"));

            int noun, verb = 0;
            var found = false;
            for (noun = 0; noun < 100; noun++)
            {
                for (verb = 0; verb < 100; verb++)
                {
                    program[1] = noun;
                    program[2] = verb;
                    _computer.Execute(program);

                    if (_computer.GetMemory(0) == 19690720)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            var answer = 100 * noun + verb;
            return answer.ToString();
        }
    }
}
