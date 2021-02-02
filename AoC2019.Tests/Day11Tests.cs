using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AoC2019.Tests
{
    [TestClass]
    public class Day11Tests : DayTestsBase<Day11.Day11>
    {
        private static readonly string AnswerPart2 = Environment.NewLine +
            "╔═══════════════════════════════════════════╗" + Environment.NewLine +
            "║   ██ ████ █    ████ ████  ██  █  █ ███    ║" + Environment.NewLine +
            "║    █ █    █    █    █    █  █ █  █ █  █   ║" + Environment.NewLine +
            "║    █ ███  █    ███  ███  █    ████ █  █   ║" + Environment.NewLine +
            "║    █ █    █    █    █    █ ██ █  █ ███    ║" + Environment.NewLine +
            "║ █  █ █    █    █    █    █  █ █  █ █      ║" + Environment.NewLine +
            "║  ██  ████ ████ ████ █     ███ █  █ █      ║" + Environment.NewLine +
            "╚═══════════════════════════════════════════╝";

        public Day11Tests() : base("2172", AnswerPart2) { }
    }
}
