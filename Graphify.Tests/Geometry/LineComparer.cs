using System.Diagnostics.CodeAnalysis;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class LineComparer : IEqualityComparer<Line>
    {
        private readonly IEqualityComparer<Point> _comparer = new PointComparer();
        public bool Equals(Line? x, Line? y) => x!.ControlPoints.SequenceEqual(y!.ControlPoints, _comparer);
        public int GetHashCode([DisallowNull] Line obj) => throw new NotImplementedException();
    }
}
