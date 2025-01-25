using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometryFactory
{
    public IFigure Create(ObjectType type, Point[] points);
    public Point Create(Vector2 points);
}

public enum ObjectType
{
    Circle,
    Line,
    CubicBezier
}
