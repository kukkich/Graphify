using System.Diagnostics.CodeAnalysis;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class PointComparer : IEqualityComparer<Point>
    {

        //public bool Equals(Point? actual, Point? expected) => (actual.X == expected.X) && (actual.Y == expected.Y) ? true : false;
        public bool Equals(Point? actual, Point? expected) => (Math.Abs(actual!.X - expected!.X) < 1E-07) && (Math.Abs(actual.Y - expected.Y) < 1E-07);
        public int GetHashCode([DisallowNull] Point obj) => throw new NotImplementedException();
    }
}
