using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.IO.Extension;

namespace Graphify.IO.Extension;

public static class ListExtension
{
    public static List<Vector2> ToListVector2(this List<Point> points)
        => points.Select(PointExtension.ToVector2).ToList();

    public static double[] ToArrayCoordinates(this List<Point> points)
        => points.SelectMany(PointExtension.ToArray).ToArray();
}
