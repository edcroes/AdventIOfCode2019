namespace AoC2019.Common.IntCodeComputer.Instructions
{
    public struct Parameter
    {
        public int Value { get; set; }
        public ParameterMode Mode { get; set; }

        public int GetValue(IntCodeComputer computer)
        {
            return Mode == ParameterMode.PositionMode ? computer.GetMemory(Value) : Value;
        }
    }
}