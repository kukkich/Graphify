using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphify.Geometry.GeometricObjects.Points;
using NUnit.Framework.Constraints;

namespace Graphify.Tests.Geometry
{
    internal class PointComparer : IEqualityComparer<Point>
    {

        public bool Equals(Point? actual, Point? expected) => (actual.X == expected.X) && (actual.Y == expected.Y)? true : false ;
        public int GetHashCode([DisallowNull] Point obj) => throw new NotImplementedException();
    }
}
