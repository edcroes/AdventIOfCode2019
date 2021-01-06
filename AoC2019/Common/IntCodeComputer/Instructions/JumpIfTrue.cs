namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct JumpIfTrue : IInstruction
    {
        public int Length => 3;

        public void Execute(IntCodeComputer computer, Parameter[] parameters)
        {
            if (parameters[0].GetValue(computer) != 0)
            {
                computer.MoveInstructionPointer(parameters[1].GetValue(computer));
            }
        }
    }
}
