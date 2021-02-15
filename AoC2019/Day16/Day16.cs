using System;
using System.IO;
using System.Linq;

namespace AoC2019.Day16
{
    public class Day16 : IMDay
    {
        private static readonly int[] BasePattern = new [] { 0, 1, 0, -1 };
        private const int ShiftBasePatternLeft = 1;
        private const int ZeroInAsciiTable = 48;

        public string GetAnswerPart1()
        {
            var signal = File.ReadAllText("Day16\\input.txt")
                .Trim()
                .ToCharArray()
                .Select(c => (int)c - ZeroInAsciiTable)
                .ToArray();

            for (var phase = 1; phase <= 100; phase++)
            {
                signal = CleanWithFlawedFrequencyTransmission(signal);
            }

            return string.Join("", signal.Take(8));
        }

        public string GetAnswerPart2()
        {
            // Because of the nature of the repeating pattern, the second half of the numbers are easy to predict.
            // Given the signal 123456 makes predicting the next numbers for 4 5 and 6 easy and doesn't involve the previous numbers.
            // 4 (pattern: 000111) =>  1*0 + 2*0 + 3*0 + 4*1 + 5*1 + 6*1  =>  4*1 + 5*1 + 6*1
            // 5 (pattern: 000011) =>  1*0 + 2*0 + 3*0 + 4*0 + 5*1 + 6*1  =>  5*1 + 6*1
            // 6 (pattern: 000001) =>  1*0 + 2*0 + 3*0 + 4*0 + 5*0 + 6*1  =>  6*1
            //
            // This means that the last number will never change.
            // If we work this back from end to start you'll get:
            // 6 => 6
            // 5 => (5*1 + 6*1) % 10 => 1
            // 4 => (4*1 + 5*1 + 6*1) % 10 => 5
            //
            // To make this easier we can see that:
            // 6 => 6
            // 5 => (5*1 + previous number which is 6) % 10 => 1
            // 4 => (4*1 + previous number which is 1) % 10 => 5
            //
            // The reason that this knowledge will help is is because of the fact that the offset given in our input lies in the second half of the repeated signal.
            // In this case the offset is 5.973.181 where the total length will be 6.500.000. This means we can use this knowledge to do the easy calculations :).
            // Since we also saw that the numbers are only affected by the numbers that er right from the current number we also won't need the part before the offset.

            var numberOfRepeats = 10000;
            var signalString = File.ReadAllText("Day16\\input.txt").Trim();
            var messageOffset = int.Parse(signalString.Substring(0, 7));

            var signal = signalString
                .ToCharArray()
                .Select(c => (int)c - ZeroInAsciiTable)
                .ToArray();

            var signalPartToConsiderLength = signal.Length * numberOfRepeats - messageOffset;
            var signalPartToConsider = new int[signalPartToConsiderLength];

            var partialPartLength = signalPartToConsiderLength % signal.Length;
            Array.Copy(signal, signal.Length - partialPartLength, signalPartToConsider, 0, partialPartLength);

            for (var repeats = 0; repeats < (signalPartToConsiderLength / signal.Length); repeats++)
            {
                Array.Copy(signal, 0, signalPartToConsider, repeats * signal.Length + partialPartLength, signal.Length);
            }

            for (var phase = 1; phase <= 100; phase++)
            {
                signalPartToConsider = CleanWithFlawedFrequencyTransmissionPart2(signalPartToConsider);
            }

            return string.Join("", signalPartToConsider.Take(8));
        }

        private static int[] CleanWithFlawedFrequencyTransmission(int[] signal)
        {
            var result = new int[signal.Length];

            for (var i = 0; i < signal.Length; i++)
            {
                var value = 0;
                for (var j = 0; j < signal.Length; j++)
                {
                    var patternIndex = (j + ShiftBasePatternLeft) / (i + 1) % BasePattern.Length;
                    value += signal[j] * BasePattern[patternIndex];
                }

                result[i] = Math.Abs(value % 10);
            }

            return result;
        }

        private static int[] CleanWithFlawedFrequencyTransmissionPart2(int[] signal)
        {
            var result = new int[signal.Length];
            result[signal.Length - 1] = signal[signal.Length - 1];

            for (var i = result.Length - 2; i >= 0; i--)
            {
                result[i] = (signal[i] + result[i + 1]) % 10;
            }

            return result;
        }
    }
}
