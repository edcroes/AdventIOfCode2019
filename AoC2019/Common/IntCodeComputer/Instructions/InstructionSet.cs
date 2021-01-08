using System;
using System.Collections.Generic;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public class InstructionSet : Dictionary<int, IInstruction>
    {
        public static InstructionSet CreateDefaultInstructionSet(Func<long> getInput, Action<long> setOutput)
        {
            return new InstructionSet
            {
                { 1, new Add() },
                { 2, new Multiply() },
                { 3, new Input(getInput) },
                { 4, new Output(setOutput) },
                { 5, new JumpIfTrue() },
                { 6, new JumpIfFalse() },
                { 7, new LessThan() },
                { 8, new Equals() },
                { 99, new Halt() }
            };
        }
    }
}
