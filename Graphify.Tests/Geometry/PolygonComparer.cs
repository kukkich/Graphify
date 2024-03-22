using System.Diagnostics.CodeAnalysis;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Polygons;

namespace Graphify.Tests.Geometry
{
    internal class PolygonComparer : IEqualityComparer<Polygon>
    {
        private readonly IEqualityComparer<Point> _comparer = new PointComparer();
        public bool Equals(Polygon? x, Polygon? y) => x.ControlPoints.SequenceEqual(y.ControlPoints, _comparer);
        public int GetHashCode([DisallowNull] Polygon obj) => throw new NotImplementedException();
    }
}
