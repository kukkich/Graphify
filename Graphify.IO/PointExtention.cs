using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO;

public static class PointExtention
{
    public static Vector2 ToVector2(this Point point)
            => new(point.X, point.Y);

    public static double[] ToArray(this Point point) 
            => [point.X, point.Y];
}
