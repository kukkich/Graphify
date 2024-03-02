using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO;

public static class PointExtention
{
    public static Vector2 ToVector2(this Point point) 
            => new(point.X, point.Y);

    public static List<Vector2> ToListVector2(this List<Point> points) 
            => points.Select(point => ToVector2(point)).ToList();
}
