using System;

namespace AoC2019.Common
{
    public static class IntExtensions
    {
        public static int GetNthDigit(this int value, int n)
        {
            return value / (int)Math.Pow(10, n) % 10;
        }
    }
}
