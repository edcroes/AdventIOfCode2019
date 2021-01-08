using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AoC2019.Tests
{
    [TestClass]
    public class Day08Tests : DayTestsBase<Day08.Day08>
    {
        private static readonly string AnswerPart2 = Environment.NewLine +
            "╔═════════════════════════╗" + Environment.NewLine +
            "║███  █  █ ███  ████ █  █ ║" + Environment.NewLine +
            "║█  █ █  █ █  █ █    █  █ ║" + Environment.NewLine +
            "║█  █ ████ █  █ ███  █  █ ║" + Environment.NewLine +
            "║███  █  █ ███  █    █  █ ║" + Environment.NewLine +
            "║█    █  █ █    █    █  █ ║" + Environment.NewLine +
            "║█    █  █ █    ████  ██  ║" + Environment.NewLine +
            "╚═════════════════════════╝";

        public Day08Tests() : base("1452", AnswerPart2) { }
    }
}
