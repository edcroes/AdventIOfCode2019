using System;
using System.Linq;
using AoC2019.Common.IntCodeComputer.Instructions;

namespace AoC2019.Common.IntCodeComputer
{
    public class IntCodeComputer
    {
        private long _instructionPointer;
        private long[] _memory;
        private readonly InstructionSet _instructionSet;
        private int? _fixedMemorySize;

        public long RelativeBase { get; set; }

        public IntCodeComputer(InstructionSet instructionSet, int? fixedMemorySize = null)
        {
            _instructionSet = instructionSet;
            _fixedMemorySize = fixedMemorySize;
        }

        public void Execute(int[] program)
        {
            Execute(program.Select(i => (long)i).ToArray());
        }

        public void Execute(long[] program)
        {
            Reset();

            var _memorySize = _fixedMemorySize ?? program.Length;
            _memory = new long[_memorySize];
            Array.Copy(program, _memory, program.Length);

            while (true)
            {
                var instruction = DecodeInstruction(_memory[_instructionPointer]);
                var parameters = GetInstructionParameters(instruction.Length);
                _instructionPointer += instruction.Length;

                if (instruction is Halt)
                {
                    break;
                }

                instruction.Execute(this, parameters);
            }
        }

        public long GetMemory(long address)
        {
            AssertMemoryIndex(address);
            return _memory[address];
        }

        public void SetMemory(long address, long value)
        {
            AssertMemoryIndex(address);
            _memory[address] = value;
        }

        public void MoveInstructionPointer(long newAddress)
        {
            AssertMemoryIndex(newAddress);
            _instructionPointer = newAddress;
        }

        private IInstruction DecodeInstruction(long instruction)
        {
            var opcode = (int)(instruction % 100);
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

        private void AssertMemoryIndex(long index)
        {
            if (index < 0 || index >= _memory.Length)
            {
                throw new StackOverflowException("Oops.... you're outside of the memory bounds");
            }
        }

        private void Reset()
        {
            _instructionPointer = 0;
            RelativeBase = 0;
        }

        public static long[] ParseProgram(string program) => program.Split(",").Select(i => long.Parse(i)).ToArray();
    }
}
