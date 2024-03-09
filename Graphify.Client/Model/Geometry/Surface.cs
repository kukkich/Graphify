using System.Numerics;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Geometry;

public class Surface : IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects => _objects;
    public IEnumerable<IFigure> Figures => _figures;
    public IEnumerable<Point> Points => _points;

    private readonly HashSet<IGeometricObject> _objects = [];
    private readonly HashSet<IFigure> _figures = [];
    private readonly HashSet<Point> _points = [];

    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision) => throw new NotImplementedException();

    public Point? TryGetClosestPoint(Vector2 point, double precision) => throw new NotImplementedException();

    public IFigure? TryGetClosestFigure(Vector2 point, double precision) => throw new NotImplementedException();

    public void AddObject(IGeometricObject newObject)
    {
        _objects.Add(newObject);

        if (newObject is Point point)
        {
            _points.Add(point);
        }
        else if(newObject is IFigure figure)
        {
            _figures.Add(figure);
        }
        
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

    public bool TryRemove(IGeometricObject target)
    {
        if (!_objects.Remove(target))
        {
            return false;
        }

        if (_points.Remove((Point)target))
        {
            return true;
        }

        if (_figures.Remove((IFigure)target))
        {
            return true;
        }

        throw new ArgumentException("Target object not found");
    }

    public void Clear()
    {
        _objects.Clear();
        _figures.Clear();
        _points.Clear();
    }
}
