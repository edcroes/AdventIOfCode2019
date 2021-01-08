using AoC2019.Common;
using System.Linq;

namespace AoC2019.Day04
{
    public class Day04 : IMDay
    {
        public string GetAnswerPart1()
        {
            var min = 197487;
            var max = 673251;
            var validPasswordCount = 0;

            for (;min <= max; min++)
            {
                if (IsPasswordValid(min))
                {
                    validPasswordCount++;
                }
            }

            return validPasswordCount.ToString();
        }

        public string GetAnswerPart2()
        {
            var min = 197487;
            var max = 673251;
            var validPasswordCount = 0;

            for (; min <= max; min++)
            {
                if (IsPasswordValid(min, true))
                {
                    validPasswordCount++;
                }
            }

            return validPasswordCount.ToString();
        }

        private static bool IsPasswordValid(int password, bool maxTwoInGroup = false)
        {
            var length = password.DigitCount();
            var foundDigits = new int[10];
            var previous = -1;

            for (var i = length - 1; i >= 0; i--)
            {
                var current = password.GetNthDigit(i);
                foundDigits[current]++;
                if (current < previous) return false;
                previous = current;
            }

            return maxTwoInGroup
                ? foundDigits.Any(d => d == 2)
                : foundDigits.Any(d => d >= 2);
        }
    }
}
