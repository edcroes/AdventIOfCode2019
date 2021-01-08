namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct Parameter
    {
        public long Value { get; set; }
        public ParameterMode Mode { get; set; }

        public long GetValue(IntCodeComputer computer)
        {
            return Mode switch
            {
                ParameterMode.PositionMode => computer.GetMemory(Value),
                ParameterMode.RelativeMode => computer.GetMemory(computer.RelativeBase + Value),
                _ => Value
            };
        }
    }
}