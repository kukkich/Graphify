using System.Diagnostics.CodeAnalysis;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class BezierComparer : IEqualityComparer<CubicBezierCurve>
    {
        private readonly IEqualityComparer<Point> _comparer = new PointComparer();
        public bool Equals(CubicBezierCurve? x, CubicBezierCurve? y) => x.ControlPoints.SequenceEqual(y.ControlPoints, _comparer);
        public int GetHashCode([DisallowNull] CubicBezierCurve obj) => throw new NotImplementedException();
    }
}
