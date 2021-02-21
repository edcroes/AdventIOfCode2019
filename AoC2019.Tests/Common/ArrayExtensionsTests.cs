using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2019.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2019.Tests.Common
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        [TestMethod]
        public void IndexOfForNonContainingIntArray()
        {
            var first = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var second = new[] { 2, 3, 4, 5, 7 };

            var index = first.IndexOf(second);

            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfForContainingIntArrayAtEnd()
        {
            var first = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var second = new[] { 5, 6, 7 };

            var index = first.IndexOf(second);

            Assert.AreEqual(5, index);
        }

        [TestMethod]
        public void IndexOfForContainingIntArrayAtStart()
        {
            var first = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var second = new[] { 0, 1, 2 };

            var index = first.IndexOf(second);

            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void ReplaceNothing()
        {
            var first = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var second = new[] { 0, 2 };
            var replacement = new[] { 8, 9 };
            var expectedResult = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            var result = first.Replace(second, replacement);

            Assert.AreEqual(expectedResult.Length, result.Length);
            Assert.IsTrue(expectedResult.AreEqual(result));
        }

        [TestMethod]
        public void ReplaceAtEnd()
        {
            var first = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var second = new[] { 5, 6, 7 };
            var replacement = new[] { 8, 9 };
            var expectedResult = new[] { 0, 1, 2, 3, 4, 8, 9 };

            var result = first.Replace(second, replacement);

            Assert.AreEqual(expectedResult.Length, result.Length);
            Assert.IsTrue(expectedResult.AreEqual(result));
        }

        [TestMethod]
        public void ReplaceAtBeginning()
        {
            var first = new[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var second = new[] { 0, 1, 2 };
            var replacement = new[] { 8, 9 };
            var expectedResult = new[] { 8, 9, 3, 4, 5, 6, 7 };

            var result = first.Replace(second, replacement);

            Assert.AreEqual(expectedResult.Length, result.Length);
            Assert.IsTrue(expectedResult.AreEqual(result));
        }

        [TestMethod]
        public void ReplaceTwice()
        {
            var first = new[] { 0, 1, 2, 3, 0, 1, 2, 3, 6, 7 };
            var second = new[] { 1, 2, 3 };
            var replacement = new[] { 8, 9 };
            var expectedResult = new[] { 0, 8, 9, 0, 8, 9, 6, 7 };

            var result = first.Replace(second, replacement);

            Assert.AreEqual(expectedResult.Length, result.Length);
            Assert.IsTrue(expectedResult.AreEqual(result));
        }

        [TestMethod]
        public void SplitNothing()
        {
            var source = new[] { "A", "B", "C", "D" };
            var expectedResult = new[] { "A", "B", "C", "D" };

            var result = source.Split("E");

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(expectedResult.AreEqual(result[0]));
        }

        [TestMethod]
        public void SplitAtEnd()
        {
            var source = new[] { "A", "B", "C", "D" };
            var expectedResult = new[] { "A", "B", "C" };

            var result = source.Split("D");

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(expectedResult.AreEqual(result[0]));
        }

        [TestMethod]
        public void SplitAtBeginning()
        {
            var source = new[] { "A", "B", "C", "D" };
            var expectedResult = new[] { "B", "C", "D" };

            var result = source.Split("A");

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(expectedResult.AreEqual(result[0]));
        }

        [TestMethod]
        public void SplitTwice()
        {
            var source = new[] { "A", "B", "C", "D", "B", "E", "F" };
            var expectedResult = new string[][]
            {
                new [] { "A" },
                new [] { "C", "D" },
                new [] { "E", "F" }
            };

            var result = source.Split("B");

            Assert.AreEqual(expectedResult.Length, result.Length);

            for (var i = 0; i < result.Length; i++)
            {
                Assert.IsTrue(expectedResult[i].AreEqual(result[i]));
            }
        }

        [TestMethod]
        public void SplitMultiple()
        {
            var source = new[] { "A", "B", "C", "D", "B", "E", "F", "G" };
            var expectedResult = new string[][]
            {
                new [] { "A" },
                new [] { "C", "D" },
                new [] { "E" },
                new [] { "G" }
            };

            var result = source.Split("B", "F");

            Assert.AreEqual(expectedResult.Length, result.Length);

            for (var i = 0; i < result.Length; i++)
            {
                Assert.IsTrue(expectedResult[i].AreEqual(result[i]));
            }
        }

        [TestMethod]
        public void SplitEverything()
        {
            var source = new[] { "A", "B", "C" };

            var result = source.Split("A", "B", "C");

            Assert.AreEqual(0, result.Length);
        }
    }
}
