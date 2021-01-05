using System;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public class Output : IInstruction
    {
        private Action<int> _setOutput;
        public int Opcode => 4;
        public int Length => 2;

        public Output(Action<int> setOutput)
        {
            _setOutput = setOutput;
        }

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            _setOutput(computer.GetMemory(parameters[0].Value));
        }
    }
}
