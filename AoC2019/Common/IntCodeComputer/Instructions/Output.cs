using System;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct Output : IInstruction
    {
        private readonly Action<long> _setOutput;
        public int Length => 2;

        public Output(Action<long> setOutput)
        {
            _setOutput = setOutput;
        }

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            var output = parameters[0].GetValue(computer);
            _setOutput(output);
        }
    }
}
