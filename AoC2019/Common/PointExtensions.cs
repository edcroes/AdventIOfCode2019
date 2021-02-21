using System.Drawing;

namespace AoC2019.Common
{
    public static class PointExtensions
    {
        public static Point Add(this Point first, Point second) => new Point(first.X + second.X, first.Y + second.Y);
    }
}
