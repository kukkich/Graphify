using System.Numerics;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Core.Geometry;

public class GeometryContext : IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects { get { return _objects; } }
    public IEnumerable<IFigure> Figures  { get { return _figures; } }
    public IEnumerable<Point> Points  { get { return _points; } }

    private List<IGeometricObject> _objects = [];
    private List<IFigure> _figures = [];
    private List<Point> _points = [];
    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision) => throw new NotImplementedException();

    public Point? TryGetClosestPoint(Vector2 point, double precision) => throw new NotImplementedException();

    public IFigure? TryGetClosestFigure(Vector2 point, double precision) => throw new NotImplementedException();

    protected void AddObject(IGeometricObject newObject)
    {
        _objects.Add(newObject);
    }

    public void AddPoint(Point newPoint)
    {
        _points.Add(newPoint);
        AddObject(newPoint);
    }

    public void AddFigure(IFigure newFigure)
    {
        _figures.Add(newFigure);
        AddObject(newFigure);
    }

    public bool TryRemove(IGeometricObject target) => throw new NotImplementedException();
}
