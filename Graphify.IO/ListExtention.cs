using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO;

public static class ListExtention
{
    public static List<Vector2> ToVector2(this List<Point> points)
        => points.Select(point => PointExtention.ToVector2(point)).ToList();
}
