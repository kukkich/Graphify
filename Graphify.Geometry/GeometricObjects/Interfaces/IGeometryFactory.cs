using System.Numerics;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometryFactory
{
    public Point CreatePoint(Vector2 position);
    public Circle CreateCircle(Vector2 center, double radius);
    public BezierCurve CreateBezierCurve(IEnumerable<Vector2> points);

    public IFigure Create(ObjectType type, Point[] points);
    public Point Create(Vector2 points);
}

public enum ObjectType
{
    Circle,
    Polygon,
}
