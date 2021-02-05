using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoC2019.Common;
using AoC2019.Common.Maps;

namespace AoC2019.Day12
{
    public class Day12 : IMDay
    {
        private const int X = 0;
        private const int Y = 1;
        private const int Z = 2;
        private static readonly Regex PointRegex = new Regex("^<x=(-{0,1}\\d+), y=(-{0,1}\\d+), z=(-{0,1}\\d+)>$");
        private List<Moon> _moons = new List<Moon>();

        public string GetAnswerPart1()
        {
            InitializeMoons();

            for (var i = 0; i < 1000; i++)
            {
                TakeTimeStep(X);
                TakeTimeStep(Y);
                TakeTimeStep(Z);
            }

            return _moons.Sum(m => m.TotalEnergy).ToString();
        }

        public string GetAnswerPart2()
        {
            InitializeMoons();

            // Assumption here is that the previous state was at TimeStep 0 (which was true for my input)
            var firstRepeatX = GetFirstRepeatingPositions(X);
            var firstRepeatY = GetFirstRepeatingPositions(Y);
            var firstRepeatZ = GetFirstRepeatingPositions(Z);

            var result = AoCMath.LeastCommonMultiple(firstRepeatX, firstRepeatY);
            result = AoCMath.LeastCommonMultiple(result, firstRepeatZ);

            return result.ToString();
        }

        private long GetFirstRepeatingPositions(int pairIndex)
        {
            HashSet<UniverseState> previousStates = new();
            Dictionary<int, int> timeStepPairs = new();

            var currentTimeStep = 0;
            var currentState = new UniverseState { TimeStep = currentTimeStep, Moons = _moons.Select(m => m.Pairs[pairIndex]).ToArray() };

            while (!previousStates.Contains(currentState))
            {
                previousStates.Add(currentState);
                currentTimeStep++;

                TakeTimeStep(pairIndex);
                currentState = new UniverseState { TimeStep = currentTimeStep, Moons = _moons.Select(m => m.Pairs[pairIndex]).ToArray() };
            }

            return currentTimeStep;
        }

        private int GetMoonsHashCode(int pairIndex)
        {
            HashCode hash = new();
            foreach (var moon in _moons)
            {
                hash.Add(moon.Pairs[pairIndex]);
            }

            return hash.ToHashCode();
        }

        private void InitializeMoons()
        {
            _moons.Clear();

            var moonPositions = ParseInput(File.ReadAllLines("Day12\\input.txt").Where(l => !string.IsNullOrWhiteSpace(l)));
            foreach (var position in moonPositions)
            {
                _moons.Add(new Moon
                {
                    Pairs = new[] { new Point(position.X, 0), new Point(position.Y, 0), new Point(position.Z, 0) }
                });
            }
        }

        private void TakeTimeStep(int pairIndex)
        {
            ApplyGravity(pairIndex);
            ApplyVelocity(pairIndex);
        }

        private void ApplyGravity(int pairIndex)
        {
            foreach (var moon in _moons)
            {
                foreach(var other in _moons.Where(m => m != moon))
                {
                    CalculateNewVelocity(moon, other, pairIndex);
                }
            }
        }

        private static void CalculateNewVelocity(Moon first, Moon second, int pairIndex) =>
            first.Pairs[pairIndex].Y = first.Pairs[pairIndex].X.CompareTo(second.Pairs[pairIndex].X) * -1 + first.Pairs[pairIndex].Y;

        private void ApplyVelocity(int pairIndex)
        {
            foreach (var moon in _moons)
            {
                moon.Pairs[pairIndex].X = moon.Pairs[pairIndex].X + moon.Pairs[pairIndex].Y;
            }
        }

        private static Point3D[] ParseInput(IEnumerable<string> positions) => positions.Select(ParsePoint).ToArray();

        private static Point3D ParsePoint(string point)
        {
            var matches = PointRegex.Match(point);
            var x = int.Parse(matches.Groups[1].Value);
            var y = int.Parse(matches.Groups[2].Value);
            var z = int.Parse(matches.Groups[3].Value);

            return new Point3D(x, y, z);
        }

        private class Moon
        {
            public Point[] Pairs { get; set; }

            public int PotentialEnergy => Pairs.Sum(p => Math.Abs(p.X));
            public int KineticEnergy => Pairs.Sum(p => Math.Abs(p.Y));
            public int TotalEnergy => PotentialEnergy * KineticEnergy;
        }

        private class UniverseState : IEquatable<UniverseState>
        {
            private int _hashCode;
            public long TimeStep { get; init; }
            public Point[] Moons { get; init; }

            public override bool Equals(object obj) => Equals(obj as UniverseState);

            public bool Equals(UniverseState other) =>
                    other != null &&
                    GetHashCode() == other.GetHashCode() &&
                    Moons.AreEqual(other.Moons);

            public override int GetHashCode()
            {
                if (_hashCode == 0)
                {
                    var hash = new HashCode();
                    foreach (var moon in Moons)
                    {
                        hash.Add(moon);
                    }
                    _hashCode = hash.ToHashCode();
                }

                return _hashCode;
            }
        }
    }
}
