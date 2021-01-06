using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2019.Common;
using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;

namespace AoC2019.Day07
{
    public class Day07 : IMDay
    {
        public string GetAnswerPart1()
        {
            BlockingCollection<int> input = new();
            BlockingCollection<int> output = new();
            var computer = CreateComputer(input, output);

            var program = IntCodeComputer.ParseProgram(File.ReadAllText("Day07\\input.txt"));
            var phaseSettings = new[] { 0, 1, 2, 3, 4 };
            var phaseOptions = phaseSettings.GetPermutations();

            var highestOutput = 0;
            foreach (var option in phaseOptions)
            {
                var lastOutput = 0;
                foreach (var phase in option)
                {
                    input.Add(phase);
                    input.Add(lastOutput);
                    computer.Execute(program);
                    lastOutput = output.Take();
                }

                if (lastOutput > highestOutput)
                {
                    highestOutput = lastOutput;
                }
            }

            return highestOutput.ToString();
        }

        public string GetAnswerPart2()
        {
            BlockingCollection<int> inputA = new();
            BlockingCollection<int> inputB = new();
            BlockingCollection<int> inputC = new();
            BlockingCollection<int> inputD = new();
            BlockingCollection<int> inputE = new();

            var ampA = CreateComputer(inputA, inputB);
            var ampB = CreateComputer(inputB, inputC);
            var ampC = CreateComputer(inputC, inputD);
            var ampD = CreateComputer(inputD, inputE);
            var ampE = CreateComputer(inputE, inputA);

            var program = IntCodeComputer.ParseProgram(File.ReadAllText("Day07\\input.txt"));
            var phaseSettings = new[] { 5, 6, 7, 8, 9 };
            var phaseOptions = phaseSettings.GetPermutations();

            var highestOutput = 0;
            foreach (var option in phaseOptions)
            {
                inputA.Add(option[0]);
                inputA.Add(0);
                inputB.Add(option[1]);
                inputC.Add(option[2]);
                inputD.Add(option[3]);
                inputE.Add(option[4]);

                var tasks = new[] {
                    Task.Run(() => ampA.Execute(program)),
                    Task.Run(() => ampB.Execute(program)),
                    Task.Run(() => ampC.Execute(program)),
                    Task.Run(() => ampD.Execute(program)),
                    Task.Run(() => ampE.Execute(program))
                };
                Task.WaitAll(tasks);

                var lastOutput = inputA.Take();
                if (lastOutput > highestOutput)
                {
                    highestOutput = lastOutput;
                }
            }

            return highestOutput.ToString();
        }

        private static IntCodeComputer CreateComputer(BlockingCollection<int> input, BlockingCollection<int> output)
        {
            var instructions = InstructionSet.CreateDefaultInstructionSet(() => input.Take(), i => output.Add(i));
            return new IntCodeComputer(instructions);
        }
    }
}
