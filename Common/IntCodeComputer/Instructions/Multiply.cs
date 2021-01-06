using System;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct Multiply : IInstruction
    {
        public int Length => 4;

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            var left = parameters[0].GetValue(computer);
            var right = parameters[1].GetValue(computer);
            var result = left * right;
            computer.SetMemory(parameters[2].Value, result);
        }
    }
}
