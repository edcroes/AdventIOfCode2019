using System;
using System.Drawing;

namespace AoC2019.Common.Maps
{
    public struct Line
    {
        public Point From { get; }
        public Point To { get; }

        public Line(Point from, Point to)
        {
            From = from;
            To = to;
        }

        public PointF? GetIntersectionWithLine(Line other)
        {
            var thisA = From.X - To.X;
            var thisB = From.Y - To.Y;
            var thisC = From.X * To.Y - From.Y * To.X;

            var otherA = other.From.X - other.To.X;
            var otherB = other.From.Y - other.To.Y;
            var otherC = other.From.X * other.To.Y - other.From.Y * other.To.X;

            var determinant = thisA * otherB - thisB * otherA;

            if (determinant == 0)
            {
                return null;
            }

            var x = (thisC * otherA - thisA * otherC) / (float)determinant;
            var y = (thisC * otherB - thisB * otherC) / (float)determinant;

            return new PointF(x, y);
        }

        public PointF? GetIntersectionWithLineSegment(Line other)
        {
            var intersectionAt = GetIntersectionWithLine(other);
            if (intersectionAt == null)
            {
                return null;
            }

            return IsPointInLineRectangle(intersectionAt.Value) && other.IsPointInLineRectangle(intersectionAt.Value)
                ? intersectionAt
                : null;
        }

        private bool IsPointInLineRectangle(PointF point) =>
                point.X >= Math.Min(From.X, To.X) &&
                point.X <= Math.Max(From.X, To.X) &&
                point.Y >= Math.Min(From.Y, To.Y) &&
                point.Y <= Math.Max(From.Y, To.Y);
    }
}
