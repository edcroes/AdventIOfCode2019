using System;
using System.Collections.Generic;
using AoC2019.Common.IntCodeComputer.Instructions;

namespace AoC2019.Common.IntCodeComputer
{
    public class IntCodeComputer
    {
        private int instructionPointer;
        private int[] _memory;
        private readonly Dictionary<int, IInstruction> _instructionSet;
        private const int HaltOpcode = 99;

        public IntCodeComputer(Dictionary<int, IInstruction> instructionSet)
        {
            _instructionSet = instructionSet;
        }

        public void Execute(int[] program)
        {
            instructionPointer = 0;
            _memory = new int[program.Length];
            Array.Copy(program, _memory, program.Length);

            while (true)
            {
                var instruction = DecodeInstruction(_memory[instructionPointer]);
                var parameters = GetInstructionParameters(instruction.Length);
                instructionPointer += instruction.Length;

                if (instruction.Opcode == HaltOpcode)
                {
                    break;
                }

                instruction.Execute(this, parameters);
            }
        }

        public int GetMemory(int address)
        {
            AssertProgramRegisterIndex(address);
            return _memory[address];
        }

        public int SetMemory(int address, int value)
        {
            AssertProgramRegisterIndex(address);
            return _memory[address] = value;
        }

        private IInstruction DecodeInstruction(int opcode)
        {
            if (!_instructionSet.ContainsKey(opcode))
            {
                throw new InvalidOperationException($"Opcode {opcode} is not supported");
            }

            return _instructionSet[opcode];
        }

        private int[] GetInstructionParameters(int length)
        {
            if (length <= 1)
            {
                return Array.Empty<int>();
            }

            var startParams = instructionPointer + 1;
            var endParams = instructionPointer + length;

            AssertProgramRegisterIndex(startParams);
            AssertProgramRegisterIndex(endParams - 1);

            return _memory[startParams..endParams];
        }

        private void AssertProgramRegisterIndex(int index)
        {
            if (index < 0 || index >= _memory.Length)
            {
                throw new StackOverflowException("Oops.... you're outside of the memory bounds");
            }
        }
    }
}
