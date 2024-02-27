using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Polygons;
using System.Numerics;
using Microsoft.Extensions.Logging;

namespace Graphify.Geometry.GeometricObjects;

public class GeometryFactory : IGeometryFactory
{
    private readonly ILogger<GeometryFactory> _logger;
    private readonly Dictionary<ObjectType, Func<Point[], IFigure>> _factoryMethods = [];

    public GeometryFactory(ILogger<GeometryFactory> logger)
    {
        _logger = logger;

        InitializeFactoryMethods();
    }

    private void InitializeFactoryMethods()
    {
        _factoryMethods.Add(ObjectType.Circle, (points) => new Circle());
        _factoryMethods.Add(ObjectType.Polygon, (points) => new Polygon());
        _factoryMethods.Add(ObjectType.Line, (points) => new Line(points[0], points[1], CurveStyle.Default));
        //FactoryMethods.Add(ObjectType.CubicBezier, (points, style) => new CubicBezierCurve());
    }

    public IFigure Create(ObjectType type, Point[] points)
    {
        if (_factoryMethods.TryGetValue(type, out var factoryMethod))
        {
            IFigure newFigure = factoryMethod(points);

            _logger.LogDebug($"Figure {type} was created");

            return newFigure;
        }

        throw new ArgumentException($"Invalid ObjectType {type}");
    }

    public Point Create(Vector2 points)
    {
        Point newPoint = new Point(points.X, points.Y, PointStyle.Default);

        _logger.LogDebug($"Point was created at ${points.X}, ${points.Y}");

        return newPoint;
    }
}
