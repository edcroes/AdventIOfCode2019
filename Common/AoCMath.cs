namespace AoC2019.Common
{
    public static class AoCMath
    {
        public static long Factorial(int value)
        {
            var fact = 1L;
            for (int next = value; next > 0; next--)
            {
                fact *= next;
            }
            return fact;
        }
    }
}
