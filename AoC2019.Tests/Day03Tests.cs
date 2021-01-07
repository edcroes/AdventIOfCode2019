using AoC2019.Common.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace AoC2019.Tests
{
    [TestClass]
    public class Day03Tests : DayTestsBase<Day03.Day03>
    {
        public Day03Tests() : base("375", "") { }

        [TestMethod]
        public void TestStraightLineIntersection()
        {
            var line1 = new Line(new Point(-10, 2), new Point(10, 2));
            var line2 = new Line(new Point(-2, -9), new Point(-2, 11));

            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(-2F, intersection.Value.X);
            Assert.AreEqual(2F, intersection.Value.Y);
        }

        [TestMethod]
        public void TestCrossedLineIntersection1()
        {
            var line1 = new Line(new Point(-10, -10), new Point(10, 10));
            var line2 = new Line(new Point(0, 2), new Point(2, 0));

            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(1F, intersection.Value.X);
            Assert.AreEqual(1F, intersection.Value.Y);
        }

        [TestMethod]
        public void TestCrossedLineIntersection2()
        {
            var line1 = new Line(new Point(-10, -10), new Point(10, 10));
            var line2 = new Line(new Point(2, 0), new Point(0, 2));

            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(1F, intersection.Value.X);
            Assert.AreEqual(1F, intersection.Value.Y);
        }

        [TestMethod]
        public void TestCrossedLineIntersection3()
        {
            var line1 = new Line(new Point(10, 10), new Point(-10, -10));
            var line2 = new Line(new Point(0, 2), new Point(2, 0));

            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(1F, intersection.Value.X);
            Assert.AreEqual(1F, intersection.Value.Y);
        }

        [TestMethod]
        public void TestCrossedLineIntersection4()
        {
            var line1 = new Line(new Point(10, 10), new Point(-10, -10));
            var line2 = new Line(new Point(2, 0), new Point(0, 2));

            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(1F, intersection.Value.X);
            Assert.AreEqual(1F, intersection.Value.Y);
        }

        [TestMethod]
        public void TestCrossedLineIntersection5()
        {
            var line1 = new Line(new Point(10, 10), new Point(-10, -10));
            var line2 = new Line(new Point(2, 0), new Point(1, 1));

            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(1F, intersection.Value.X);
            Assert.AreEqual(1F, intersection.Value.Y);
        }

        [TestMethod]
        public void TestCrossedLineIntersection6()
        {
            var line1 = new Line(new Point(10, 10), new Point(1, 1));
            var line2 = new Line(new Point(3, -1), new Point(2, 0));

            var lineIntersection = line1.GetIntersectionWithLine(line2);
            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(lineIntersection);
            Assert.AreEqual(1F, lineIntersection.Value.X);
            Assert.AreEqual(1F, lineIntersection.Value.Y);
            Assert.IsNull(intersection);
        }

        [TestMethod]
        public void TestCrossedWithStraightLineIntersection7()
        {
            var line1 = new Line(new Point(10, 10), new Point(-10, -10));
            var line2 = new Line(new Point(2, -1), new Point(2, 8));

            var intersection = line1.GetIntersectionWithLineSegment(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(2F, intersection.Value.X);
            Assert.AreEqual(2F, intersection.Value.Y);
        }
    }
}
