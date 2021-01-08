namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct RelativeBaseOffset : IInstruction
    {
        public int Length => 2;

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            computer.RelativeBase = parameters[0].GetValue(computer);
        }
    }
}
