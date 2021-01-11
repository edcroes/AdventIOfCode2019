using AoC2019.Common.IntCodeComputer;
using AoC2019.Common.IntCodeComputer.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Tests.Common
{
    [TestClass]
    public class IntCodeComputerTests
    {
        private IntCodeComputer _computer;
        private readonly Queue<long> _input = new();
        private readonly List<long> _output = new();

        [TestInitialize]
        public void TestInitialize()
        {
            _input.Clear();
            _output.Clear();
            var instructionSet = InstructionSet.CreateDefaultInstructionSet(() => _input.Dequeue(), o => _output.Add(o));
            _computer = new IntCodeComputer(instructionSet);
        }

        [TestMethod]
        public void OutputInstructionPositionTest()
        {
            var program = new long[] { 109, -1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(-1, _output[0]);
        }

        [TestMethod]
        public void OutputInstructionRelativeTest()
        {
            var program = new long[] { 109, -1, 204, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(109, _output[0]);
        }

        [TestMethod]
        public void OutputInstructionImmediateTest()
        {
            var program = new long[] { 109, -1, 104, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(1, _output[0]);
        }

        [TestMethod]
        public void InputInstructionPositionTest()
        {
            var program = new long[] { 109, -1, 3, 2, 4, 2, 99 };
            _input.Enqueue(100);
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(100, _output[0]);
        }

        [TestMethod]
        public void InputInstructionRelativeTest()
        {
            var program = new long[] { 109, -1, 203, 2, 4, 2, 4, 1, 99 };
            _input.Enqueue(100);
            _computer.Execute(program);

            Assert.AreEqual(2, _output.Count);
            Assert.AreEqual(203, _output[0]);
            Assert.AreEqual(100, _output[1]);
        }

        [TestMethod]
        public void InputInstructionImmediateShouldFailTest()
        {
            var program = new long[] { 109, -1, 103, 2, 4, 2, 4, 1, 99 };
            _input.Enqueue(100);

            Assert.ThrowsException<NotSupportedException>(() => _computer.Execute(program));
        }

        [TestMethod]
        public void AddInstructionPositionTest()
        {
            var program = new long[] { 109, -1, 1, 2, 3, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(3, _output[0]);
        }

        [TestMethod]
        public void AddInstructionRelativeTest()
        {
            var program = new long[] { 109, -1, 22201, 2, 3, 1, 4, 0, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(22200, _output[0]);
        }

        [TestMethod]
        public void AddInstructionImmediateTest()
        {
            var program = new long[] { 109, -1, 1101, 2, 3, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(5, _output[0]);
        }

        [TestMethod]
        public void AddInstructionImmediateShouldFailTest()
        {
            var program = new long[] { 109, -1, 11101, 2, 3, 1, 4, 1, 99 };
            Assert.ThrowsException<NotSupportedException>(() => _computer.Execute(program));
        }

        [TestMethod]
        public void MultiplyInstructionPositionTest()
        {
            var program = new long[] { 109, -1, 2, 2, 3, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(4, _output[0]);
        }

        [TestMethod]
        public void MultiplyInstructionRelativeTest()
        {
            var program = new long[] { 109, -1, 22202, 2, 3, 1, 4, 0, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(-22202, _output[0]);
        }

        [TestMethod]
        public void MultiplyInstructionImmediateTest()
        {
            var program = new long[] { 109, -1, 1102, 2, 3, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(6, _output[0]);
        }

        [TestMethod]
        public void MultiplyInstructionImmediateShouldFailTest()
        {
            var program = new long[] { 109, -1, 11102, 2, 3, 1, 4, 1, 99 };
            Assert.ThrowsException<NotSupportedException>(() => _computer.Execute(program));
        }

        [TestMethod]
        public void RelativeBaseOffsetInstructionPositionTest()
        {
            var program = new long[] { 9, 9, 22202, 3, 6, 1, 4, 0, 99, -1 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(22202, _output[0]);
        }

        [TestMethod]
        public void RelativeBaseOffsetInstructionRelativeTest()
        {
            var program = new long[] { 209, 11, 209, 14, 22202, 5, 8, 1, 4, 0, 99, -2, 1 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(22202, _output[0]);
        }

        [TestMethod]
        public void RelativeBaseOffsetInstructionImmediateTest()
        {
            var program = new long[] { 109, -1, 22202, 2, 3, 1, 4, 0, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(-22202, _output[0]);
        }

        [TestMethod]
        public void JumpIfTrueInstructionPositionDoJumpTest()
        {
            var program = new long[] { 109, -1, 5, 12, 13, 4, 14, 99, 4, 15, 99, 0, 1, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(7777, _output[0]);
        }

        [TestMethod]
        public void JumpIfTrueInstructionRelativeDoJumpTest()
        {
            var program = new long[] { 109, -1, 2205, 13, 14, 4, 14, 99, 4, 15, 99, 0, 1, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(7777, _output[0]);
        }

        [TestMethod]
        public void JumpIfTrueInstructionImmediateDoJumpTest()
        {
            var program = new long[] { 109, -1, 1105, 1, 8, 4, 14, 99, 4, 15, 99, 1, 0, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(7777, _output[0]);
        }

        [TestMethod]
        public void JumpIfTrueInstructionPositionDontJumpTest()
        {
            var program = new long[] { 109, -1, 5, 12, 13, 4, 14, 99, 4, 15, 99, 1, 0, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(8888, _output[0]);
        }

        [TestMethod]
        public void JumpIfTrueInstructionRelativeDontJumpTest()
        {
            var program = new long[] { 109, -1, 2205, 13, 14, 4, 14, 99, 4, 15, 99, 1, 0, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(8888, _output[0]);
        }

        [TestMethod]
        public void JumpIfTrueInstructionImmediateDontJumpTest()
        {
            var program = new long[] { 109, -1, 1105, 0, 8, 4, 14, 99, 4, 15, 99, 1, 0, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(8888, _output[0]);
        }

        [TestMethod]
        public void JumpIfFalseInstructionPositionDoJumpTest()
        {
            var program = new long[] { 109, -1, 6, 12, 13, 4, 14, 99, 4, 15, 99, 1, 0, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(7777, _output[0]);
        }

        [TestMethod]
        public void JumpIfFalseInstructionRelativeDoJumpTest()
        {
            var program = new long[] { 109, -1, 2206, 13, 14, 4, 14, 99, 4, 15, 99, 1, 0, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(7777, _output[0]);
        }

        [TestMethod]
        public void JumpIfFalseInstructionImmediateDoJumpTest()
        {
            var program = new long[] { 109, -1, 1106, 0, 8, 4, 14, 99, 4, 15, 99, 1, 0, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(7777, _output[0]);
        }

        [TestMethod]
        public void JumpIfFalseInstructionPositionDontJumpTest()
        {
            var program = new long[] { 109, -1, 6, 12, 13, 4, 14, 99, 4, 15, 99, 0, 1, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(8888, _output[0]);
        }

        [TestMethod]
        public void JumpIfFalseInstructionRelativeDontJumpTest()
        {
            var program = new long[] { 109, -1, 2206, 13, 14, 4, 14, 99, 4, 15, 99, 0, 1, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(8888, _output[0]);
        }

        [TestMethod]
        public void JumpIfFalseInstructionImmediateDontJumpTest()
        {
            var program = new long[] { 109, -1, 1106, 1, 8, 4, 14, 99, 4, 15, 99, 0, 1, 8, 8888, 7777 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(8888, _output[0]);
        }

        [TestMethod]
        public void LessThanInstructionTruePositionTest()
        {
            var program = new long[] { 109, -1, 7, 10, 9, 11, 4, 11, 99, 1, 0, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(1, _output[0]);
        }

        [TestMethod]
        public void LessThanInstructionTrueRelativeTest()
        {
            var program = new long[] { 109, -1, 22207, 11, 10, 12, 4, 11, 99, 1, 0, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(1, _output[0]);
        }

        [TestMethod]
        public void LessThanInstructionTrueImmediateTest()
        {
            var program = new long[] { 109, -1, 1107, 0, 1, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(1, _output[0]);
        }

        [TestMethod]
        public void LessThanInstructionFalsePositionTest()
        {
            var program = new long[] { 109, -1, 7, 10, 9, 11, 4, 11, 99, 1, 1, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(0, _output[0]);
        }

        [TestMethod]
        public void LessThanInstructionFalseRelativeTest()
        {
            var program = new long[] { 109, -1, 22207, 11, 10, 12, 4, 11, 99, 1, 1, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(0, _output[0]);
        }

        [TestMethod]
        public void LessThanInstructionFalseImmediateTest()
        {
            var program = new long[] { 109, -1, 1107, 1, 1, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(0, _output[0]);
        }

        [TestMethod]
        public void EqualsInstructionTruePositionTest()
        {
            var program = new long[] { 109, -1, 8, 10, 9, 11, 4, 11, 99, 1, 1, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(1, _output[0]);
        }

        [TestMethod]
        public void EqualsInstructionTrueRelativeTest()
        {
            var program = new long[] { 109, -1, 22208, 11, 10, 12, 4, 11, 99, 1, 1, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(1, _output[0]);
        }

        [TestMethod]
        public void EqualsInstructionTrueImmediateTest()
        {
            var program = new long[] { 109, -1, 1108, 1, 1, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(1, _output[0]);
        }

        [TestMethod]
        public void EqualsInstructionFalsePositionTest()
        {
            var program = new long[] { 109, -1, 8, 10, 9, 11, 4, 11, 99, 99, 1, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(0, _output[0]);
        }

        [TestMethod]
        public void EqualsInstructionFalseRelativeTest()
        {
            var program = new long[] { 109, -1, 22208, 11, 10, 12, 4, 11, 99, 99, 1, 7 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(0, _output[0]);
        }

        [TestMethod]
        public void EqualsInstructionFalseImmediateTest()
        {
            var program = new long[] { 109, -1, 1108, 5, 7, 1, 4, 1, 99 };
            _computer.Execute(program);

            Assert.AreEqual(1, _output.Count);
            Assert.AreEqual(0, _output[0]);
        }

        [TestMethod]
        public void EightComparerTest()
        {
            var program = new long[] { 3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99 };
            _input.Enqueue(7);
            _computer.Execute(program);
            _input.Enqueue(8);
            _computer.Execute(program);
            _input.Enqueue(9);
            _computer.Execute(program);

            Assert.AreEqual(3, _output.Count);
            Assert.AreEqual(999, _output[0]);
            Assert.AreEqual(1000, _output[1]);
            Assert.AreEqual(1001, _output[2]);
        }

        [TestMethod]
        public void CopyOfSelfTest()
        {
            var instructionSet = InstructionSet.CreateDefaultInstructionSet(() => _input.Dequeue(), o => _output.Add(o));
            _computer = new IntCodeComputer(instructionSet, 128);
            var program = new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
            _computer.Execute(program);

            Assert.IsTrue(_output.SequenceEqual(program));
        }
    }
}
