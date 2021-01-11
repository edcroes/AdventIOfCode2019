namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct LessThan : IInstruction
    {
        public int Length => 4;

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            var result = parameters[0].GetValue(computer) < parameters[1].GetValue(computer) ? 1 : 0;
            computer.SetMemory(parameters[2].GetAddress(computer), result);
        }
    }
}
