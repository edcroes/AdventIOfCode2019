using System.Runtime.InteropServices.ComTypes;

namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public interface IInstruction
    {
        int Length { get; }
        void Execute(IntCodeComputer computer, Parameter[] parameters);
    }
}
