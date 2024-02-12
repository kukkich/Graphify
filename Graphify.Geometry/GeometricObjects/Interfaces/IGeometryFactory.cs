using System.Numerics;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometryFactory
{
    public Point CreatePoint(Vector2 position);
    public Circle CreateCircle(Vector2 center, double radius);
    public BezierCurve CreateBezierCurve(IEnumerable<Vector2> points);
}
