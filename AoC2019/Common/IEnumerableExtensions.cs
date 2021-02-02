using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Common
{
    public static class IEnumerableExtensions
    {
        public static T[][] GetPermutations<T>(this IEnumerable<T> items)
        {
            if (items is null || !items.Any())
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

        public static T[][] Split<T>(this IEnumerable<T> itemsToSplit, int sizeOfResultingArrays)
        {
            if (itemsToSplit is null)
            {
                throw new ArgumentNullException(nameof(itemsToSplit));
            }

            var arrayToSplit = itemsToSplit.ToArray();
            if (arrayToSplit.Length % sizeOfResultingArrays != 0)
            {
                throw new ArgumentException("The IEnumerable cannot be divided by the size of the resulting arrays");
            }

            var parts = arrayToSplit.Length / sizeOfResultingArrays;
            var splits = new T[parts][];
            for (var part = 0; part < parts; part++)
            {
                splits[part] = arrayToSplit[(part * sizeOfResultingArrays)..((part + 1) * sizeOfResultingArrays)];
            }

            return splits;
        }

        public static T GetNextItem<T>(this IEnumerable<T> items, T currentItem)
        {
            var array = items is T[] arr ? arr : items.ToArray();
            var currentIndex = Array.IndexOf(array, currentItem);

            return array[(currentIndex + 1) % array.Length];
        }

        public static T GetPreviousItem<T>(this IEnumerable<T> items, T currentItem)
        {
            var array = items is T[] arr ? arr : items.ToArray();
            var currentIndex = Array.IndexOf(array, currentItem);

            return array[--currentIndex < 0 ? array.Length - 1 : currentIndex];
        }
    }
}
