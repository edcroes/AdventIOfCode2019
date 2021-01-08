using System;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct Input : IInstruction
    {
        private readonly Func<long> _getInput;
        public int Length => 2;

        public Input(Func<long> getInput)
        {
            _getInput = getInput;
        }

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            var input = _getInput();
            computer.SetMemory(parameters[0].Value, input);
        }
    }
}
