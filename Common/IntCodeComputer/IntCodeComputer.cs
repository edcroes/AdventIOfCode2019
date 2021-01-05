using System;
using System.Collections.Generic;
using AoC2019.Common.IntCodeComputer.Instructions;

namespace AoC2019.Common.IntCodeComputer
{
    public class IntCodeComputer
    {
        private int _instructionPointer;
        private int[] _memory;
        private readonly Dictionary<int, IInstruction> _instructionSet;
        private const int HaltOpcode = 99;

        public IntCodeComputer(Dictionary<int, IInstruction> instructionSet)
        {
            _instructionSet = instructionSet;
        }

        public void Execute(int[] program)
        {
            _instructionPointer = 0;
            _memory = new int[program.Length];
            Array.Copy(program, _memory, program.Length);

            while (true)
            {
                var instruction = DecodeInstruction(_memory[_instructionPointer]);
                var parameters = GetInstructionParameters(instruction.Length);
                _instructionPointer += instruction.Length;

                if (instruction.Opcode == HaltOpcode)
                {
                    break;
                }

                instruction.Execute(this, parameters);
            }
        }

        public int GetMemory(int address)
        {
            AssertMemoryIndex(address);
            return _memory[address];
        }

        public int SetMemory(int address, int value)
        {
            AssertMemoryIndex(address);
            return _memory[address] = value;
        }

        public void MoveInstructionPointer(int newAddress)
        {
            AssertMemoryIndex(newAddress);
            _instructionPointer = newAddress;
        }

        private IInstruction DecodeInstruction(int instruction)
        {
            var opcode = instruction % 100;
            if (!_instructionSet.ContainsKey(opcode))
            {
                throw new InvalidOperationException($"Opcode {opcode} is not supported");
            }

            return _instructionSet[opcode];
        }

        private Parameter[] GetInstructionParameters(int length)
        {
            if (length <= 1)
            {
                return Array.Empty<Parameter>();
            }

            var parameters = new Parameter[length - 1];
            var startParams = _instructionPointer + 1;
            var endParams = _instructionPointer + length - 1;

            AssertMemoryIndex(startParams);
            AssertMemoryIndex(endParams);

            for (var index = 0; index < length - 1; index ++)
            {
                var mode = (ParameterMode)_memory[_instructionPointer].GetNthDigit(2 + index);
                var value = _memory[index + startParams];
                parameters[index] = new Parameter { Value = value, Mode = mode };
            }

            return parameters;
        }

        private void AssertMemoryIndex(int index)
        {
            if (index < 0 || index >= _memory.Length)
            {
                throw new StackOverflowException("Oops.... you're outside of the memory bounds");
            }
        }
    }
}
