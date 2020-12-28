using System;
using System.Collections.Generic;

namespace AoC2019.Common.Maps
{
    public class Map3D<T>
    {
        private readonly T[,,] _map;

        public int SizeX => _map.GetLength(2);
        public int SizeY => _map.GetLength(1);
        public int SizeZ => _map.GetLength(0);

        public Map3D(int x, int y, int z)
        {
            _map = new T[z, y, x];
        }

        public void PlaceInMiddle(T[,] initialState)
        {
            var z = SizeZ / 2;
            var startY = SizeY / 2 - initialState.GetLength(0) / 2;
            var startX = SizeX / 2 - initialState.GetLength(1) / 2;

            for (int y = 0; y < initialState.GetLength(0); y++)
            {
                for (int x = 0; x < initialState.GetLength(1); x++)
                {
                    _map[z, y + startY, x + startX] = initialState[y, x];
                }
            }
        }

        public T GetValueOrDefault(Point3D point)
        {
            return GetValueOrDefault(point.X, point.Y, point.Z);
        }

        public T GetValueOrDefault(int x, int y, int z)
        {
            if (z < 0 || z >= SizeZ || y < 0 || y >= SizeY || x < 0 || x >= SizeX)
            {
                return default;
            }

            return GetValue(x, y, z);
        }

        public T GetValue(Point3D point)
        {
            return GetValue(point.X, point.Y, point.Z);
        }

        public T GetValue(int x, int y, int z)
        {
            return _map[z, y, x];
        }

        public void SetValue(int x, int y, int z, T value)
        {
            _map[z, y, x] = value;
        }

        public void SetValue(Point3D point, T value)
        {
            SetValue(point.X, point.Y, point.Z, value);
        }

        public void DistributeChaos(T aliveValue, Func<bool, int, T> getNewValue)
        {
            DistributeChaos(aliveValue, (map, point) => NumberOfNeighborsThatMatch(map, point, aliveValue), getNewValue);
        }

        public void DistributeChaos(T aliveValue, Func<Map3D<T>, Point3D, int> getNumberOfNeighborsThatMatch, Func<bool, int, T> getNewValue)
        {
            var pointsToChange = new Dictionary<Point3D, T>();

            for (int z = 0; z < SizeZ; z++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    for (int x = 0; x < SizeX; x++)
                    {
                        var currentPoint = new Point3D(x, y, z);
                        var currentValue = GetValue(currentPoint);
                        var matches = getNumberOfNeighborsThatMatch(this, currentPoint);

                        var newValue = getNewValue(aliveValue.Equals(currentValue), matches);
                        if (!newValue.Equals(currentValue))
                        {
                            pointsToChange.Add(currentPoint, newValue);
                        }
                    }
                }
            }

            foreach (var point in pointsToChange.Keys)
            {
                SetValue(point, pointsToChange[point]);
            }
        }

        public int NumberOfNeighborsThatMatch(Point3D point, T valueToMatch)
        {
            return NumberOfNeighborsThatMatch(this, point, valueToMatch);
        }

        private static int NumberOfNeighborsThatMatch(Map3D<T> map, Point3D point, T valueToMatch)
        {
            var numberOfMatches = 0;

            for (int z = Math.Max(point.Z - 1, 0); z <= point.Z + 1 && z < map.SizeZ; z++)
            {
                for (int y = Math.Max(point.Y - 1, 0); y <= point.Y + 1 && y < map.SizeY; y++)
                {
                    for (int x = Math.Max(point.X - 1, 0); x <= point.X + 1 && x < map.SizeX; x++)
                    {
                        if (z == point.Z && y == point.Y && x == point.X)
                        {
                            continue;
                        }

                        if (map.GetValue(x, y, z).Equals(valueToMatch))
                        {
                            numberOfMatches++;
                        }
                    }
                }
            }

            return numberOfMatches;
        }

        public int Count(T valueToMatch)
        {
            int count = 0;
            for (int z = 0; z < SizeZ; z++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    for (int x = 0; x < SizeX; x++)
                    {
                        if (_map[z, y, x].Equals(valueToMatch))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        public T[] GetLine(int fromX, int fromY, int fromZ, int toX, int toY, int toZ)
        {
            var row = new List<T>();

            var moveZ = fromZ == toZ ? 0 : fromZ > toZ ? -1 : 1;
            var moveY = fromY == toY ? 0 : fromY > toY ? -1 : 1;
            var moveX = fromX == toX ? 0 : fromX > toX ? -1 : 1;

            do
            {
                row.Add(_map[fromZ, fromY, fromX]);
                fromZ += moveZ;
                fromY += moveY;
                fromX += moveX;
            }
            while ((fromX <= toX && moveX == 1) || (fromX >= toX && moveX == -1) || (fromY <= toY && moveY == 1) || (fromY >= toY && moveY == -1) || (fromZ <= toZ && moveZ == 1) || (fromZ >= toZ && moveZ == -1));

            return row.ToArray();
        }
    }
}
