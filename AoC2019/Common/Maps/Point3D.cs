using System;

namespace AoC2019.Common.Maps
{
    public struct Point3D
    {
        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}
