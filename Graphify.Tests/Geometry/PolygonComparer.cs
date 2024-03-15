using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Polygons;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class PolygonComparer : IEqualityComparer<Polygon>
    {
        private readonly IEqualityComparer<Point> _comparer = new PointComparer();
        public bool Equals(Polygon? x, Polygon? y) => x.ControlPoints.SequenceEqual(y.ControlPoints, _comparer);
        public int GetHashCode([DisallowNull] Polygon obj) => throw new NotImplementedException();
    }
}
