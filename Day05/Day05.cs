using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;

namespace AoC2019.Day05
{
    public class Day05 : IMDay
    {
        private readonly IntCodeComputer _computer;
        private readonly List<int> _computerOutput = new();
        private int _systemToTest;

        public Day05()
        {
            var instructions = new Dictionary<int, IInstruction>
            {
                { 1, new Add() },
                { 2, new Multiply() },
                { 3, new Input(() => _systemToTest) },
                { 4, new Output(i => _computerOutput.Add(i)) },
                { 5, new JumpIfTrue() },
                { 6, new JumpIfFalse() },
                { 7, new LessThan() },
                { 8, new Equals() },
                { 99, new Halt() }
            };
            _computer = new IntCodeComputer(instructions);
        }

        public string GetAnswerPart1()
        {
            _systemToTest = 1;
            _computerOutput.Clear();
            var program = ParseProgram(File.ReadAllText("Day05\\input.txt"));
            _computer.Execute(program);

            return _computerOutput.Last().ToString();
        }

        public string GetAnswerPart2()
        {
            _systemToTest = 5;
            _computerOutput.Clear();
            var program = ParseProgram(File.ReadAllText("Day05\\input.txt"));
            _computer.Execute(program);

            return _computerOutput.Last().ToString();
        }

        private static int[] ParseProgram(string program) => program.Split(",").Select(i => int.Parse(i)).ToArray();


    }
}
