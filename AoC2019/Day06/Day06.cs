using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Day06
{
    public class Day06 : IMDay
    {
        private readonly Dictionary<string, string> _orbitGraph;

        public Day06()
        {
            var orbits = File.ReadAllLines("Day06\\input.txt")
                .Where(l => !string.IsNullOrEmpty(l));

            _orbitGraph = CreateOrbitGraph(orbits);
        }

        public string GetAnswerPart1()
        {
            var answer = _orbitGraph.Keys.Select(o => GetOrbitCount(o)).Sum();
            return answer.ToString();
        }

        public string GetAnswerPart2()
        {
            var orbitJumps = GetOrbitJumps("YOU", "SAN");
            return orbitJumps.ToString();
        }

        private int GetOrbitJumps(string fromObject, string toObject)
        {
            var fromOrbits = GetOrbitsBackToStart(fromObject);
            var toOrbits = GetOrbitsBackToStart(toObject);

            var firstMatchingObject = fromOrbits.First(o => toOrbits.Contains(o));
            return Array.IndexOf(toOrbits, firstMatchingObject) + Array.IndexOf(fromOrbits, firstMatchingObject);
        }

        private string[] GetOrbitsBackToStart(string spaceObject)
        {
            List<string> orbits = new();

            var parent = _orbitGraph[spaceObject];
            orbits.Add(parent);
            while (_orbitGraph.ContainsKey(parent))
            {
                parent = _orbitGraph[parent];
                orbits.Add(parent);
            }

            return orbits.ToArray();
        }

        private int GetOrbitCount(string spaceObject)
        {
            return _orbitGraph.ContainsKey(spaceObject)
                ? GetOrbitCount(_orbitGraph[spaceObject]) + 1
                : 0;
        }

        private static Dictionary<string, string> CreateOrbitGraph(IEnumerable<string> orbits)
        {
            Dictionary<string, string> orbitGraph = new();

            foreach (var orbit in orbits)
            {
                var data = orbit.Split(")");

                if (orbitGraph.ContainsKey(data[1]))
                {
                    orbitGraph[data[1]] = data[0];
                }
                else
                {
                    orbitGraph.Add(data[1], data[0]);
                }
            }

            return orbitGraph;
        }
    }
}
