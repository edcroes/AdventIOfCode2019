using System;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct Equals : IInstruction
    {
        public int Length => 4;

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            computer.SetMemory(parameters[2].Value, parameters[0].GetValue(computer) == parameters[1].GetValue(computer) ? 1 : 0);
        }
    }
}
