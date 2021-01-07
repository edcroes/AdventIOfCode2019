using System.IO;
using System.Linq;

namespace AoC2019.Day01
{
    public class Day01 : IMDay
    {
        private readonly int[] _modules;

        public Day01()
        {
            _modules = File.ReadAllLines("Day01\\input.txt")
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => int.Parse(l))
                .ToArray();
        }

        public string GetAnswerPart1()
        {
            var fuelConsumption = _modules
                .Select(m => GetFuelConsumption(m))
                .Sum();

            return fuelConsumption.ToString();
        }

        public string GetAnswerPart2()
        {
            var fuelConsumption = _modules
                .Select(m => GetTotalFuelConsumption(m))
                .Sum();

            return fuelConsumption.ToString();
        }

        private static int GetFuelConsumption(int module) => module / 3 - 2;

        private static int GetTotalFuelConsumption(int fuelUser)
        {
            var fuelRequired = GetFuelConsumption(fuelUser);
            return fuelRequired > 0
                ? fuelRequired + GetTotalFuelConsumption(fuelRequired)
                : 0;
        }
    }
}
