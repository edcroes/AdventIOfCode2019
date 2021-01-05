using System.Runtime.InteropServices.ComTypes;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public interface IInstruction
    {
        int Opcode { get; }
        int Length { get; }
        void Execute(IntCodeComputer computer, Parameter[] parameters);
    }
}
