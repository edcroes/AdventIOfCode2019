using System;

namespace AoC2019.Common
{
    public static class LongExtensions
    {
        public static int GetNthDigit(this long value, int n)
        {
            return (int)(value / (int)Math.Pow(10, n) % 10);
        }
    }
}
