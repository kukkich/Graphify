using System.Diagnostics.CodeAnalysis;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class CircleComparer : IEqualityComparer<Circle>
    {
        private readonly IEqualityComparer<Point> _comparer = new PointComparer();
        public bool Equals(Circle? x, Circle? y) => x!.ControlPoints.SequenceEqual(y!.ControlPoints, _comparer);
        public int GetHashCode([DisallowNull] Circle obj) => throw new NotImplementedException();
    }
}
