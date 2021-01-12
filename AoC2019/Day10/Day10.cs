using AoC2019.Common.Maps;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AoC2019.Day10
{
    public class Day10 : IMDay
    {
        private readonly Map<bool> _asteroidMap;
        private readonly Point[] _asteroids;

        public Day10()
        {
            _asteroidMap = ParseInput(File.ReadAllLines("Day10\\input.txt").Where(l => !string.IsNullOrEmpty(l)));
            _asteroids = GetAsteroids(_asteroidMap).ToArray();
        }

        private Point GetAsteroidWithMostLinesOfSight()
        {
            return _asteroids
                .Select(a => new KeyValuePair<Point, int>(a, GetAsteroidWithLineOfSightCount(a)))
                .OrderByDescending(a => a.Value)
                .First()
                .Key;
        }

        public string GetAnswerPart1()
        {
            var asteroid = GetAsteroidWithMostLinesOfSight();
            return GetAsteroidWithLineOfSightCount(asteroid).ToString();
        }

        public string GetAnswerPart2()
        {
            var asteroid = GetAsteroidWithMostLinesOfSight();
            var allLines = GetAllLinesFromAsteroid(asteroid)
                .OrderBy(l => TransformAngle(l.Angle))
                .ThenBy(l => l.Length);
            var allAngles = new Dictionary<double, Queue<Line>>();

            foreach (var line in allLines)
            {
                var angle = TransformAngle(line.Angle);
                if (allAngles.ContainsKey(angle))
                {
                    allAngles[angle].Enqueue(line);
                }
                else
                {
                    var queue = new Queue<Line>();
                    queue.Enqueue(line);
                    allAngles.Add(angle, queue);
                }
            }

            var currentAsteroid = 0;
            while (true)
            {
                foreach (var key in allAngles.Keys)
                {
                    if (allAngles[key].Count > 0)
                    {
                        var target = allAngles[key].Dequeue();
                        if (++currentAsteroid == 200)
                        {
                            return (target.To.X * 100 + TransformY(target.To).Y).ToString();
                        }
                    }
                }
            }
        }

        private int GetAsteroidWithLineOfSightCount(Point asteroid)
        {
            return GetAllLinesFromAsteroid(asteroid)
                .Select(l => l.Angle)
                .Distinct()
                .Count();
        }

        private IEnumerable<Line> GetAllLinesFromAsteroid(Point asteroid)
        {
            return _asteroids
                .Where(a => a != asteroid)
                .Select(a => new Line(TransformY(asteroid), TransformY(a)));
        }

        // Change Y between top-left 0,0 and bottom-left 0,0
        private Point TransformY(Point point) => new Point(point.X, _asteroidMap.SizeY - 1 - point.Y);

        //                       90              0
        //                       |               |
        // Transform from  180 --+-- 0 to  270 --+-- 90
        //                       |               |
        //                      270             180
        private double TransformAngle(double angle) => (360 - angle + 90) % 360;

        private static Map<bool> ParseInput(IEnumerable<string> input)
        {
            var array = input.ToArray();
            var map = new Map<bool>(array[0].Length, array.Length);

            for (var y = 0; y < array.Length; y++)
            {
                var chars = array[y].ToCharArray();
                for (var x = 0; x < chars.Length; x++)
                {
                    map.SetValue(x, y, chars[x] == '#');
                }
            }

            return map;
        }

        private static List<Point> GetAsteroids(Map<bool> map)
        {
            var asteroids = new List<Point>();

            for (var y = 0; y < map.SizeY; y++)
            {
                for (var x = 0; x < map.SizeX; x++)
                {
                    if (map.GetValue(x, y))
                    {
                        asteroids.Add(new Point(x, y));
                    }
                }
            }

            return asteroids;
        }
    }
}
