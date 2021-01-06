using System;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct Halt : IInstruction
    {
        public int Length => 1;

        public void Execute(IntCodeComputer computer, Parameter[] arguments)
        {
            throw new NotSupportedException("The halt opcode cannot be executed");
        }
    }
}
