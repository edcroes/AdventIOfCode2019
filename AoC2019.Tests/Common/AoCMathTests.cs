using AoC2019.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2019.Tests.Common
{
    [TestClass]
    public class AoCMathTests
    {
        [TestMethod]
        public void GreatestCommonDivisorGreatestFirst()
        {
            var result = AoCMath.GreatestCommonDivisor(48, 18);

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void GreatestCommonDivisorGreatestLast()
        {
            var result = AoCMath.GreatestCommonDivisor(18, 48);

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void LeastCommonMultipleGreatestFirst()
        {
            var result = AoCMath.LeastCommonMultiple(48, 18);

            Assert.AreEqual(144, result);
        }

        [TestMethod]
        public void LeastCommonMultipleGreatestLast()
        {
            var result = AoCMath.LeastCommonMultiple(18, 48);

            Assert.AreEqual(144, result);
        }
    }
}
