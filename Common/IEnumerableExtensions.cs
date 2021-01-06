using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Common
{
    public static class IEnumerableExtensions
    {
        public static T[][] GetPermutations<T>(this IEnumerable<T> items)
        {
            if (items == null || !items.Any())
            {
                return null;
            }

            var itemArray = items.ToArray();
            var allPermutations = new T[AoCMath.Factorial(itemArray.Length)][];
            allPermutations[0] = itemArray.ToArray();

            var indexes = new int[itemArray.Length];
            var currentPermutation = 0;
            for (int i = 1; i < itemArray.Length;)
            {
                if (indexes[i] < i)
                {
                    if ((i & 1) == 1)
                    {
                        var temp = itemArray[i];
                        itemArray[i] = itemArray[indexes[i]];
                        itemArray[indexes[i]] = temp;
                    }
                    else
                    {
                        var temp = itemArray[i];
                        itemArray[i] = itemArray[0];
                        itemArray[0] = temp;
                    }
                    allPermutations[++currentPermutation] = itemArray.ToArray();

                    indexes[i]++;
                    i = 1;
                }
                else
                {
                    indexes[i++] = 0;
                }
            }

            return allPermutations;
        }
    }
}
