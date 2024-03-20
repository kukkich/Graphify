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

    // TODO add object filter
    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision = 10)
    {
        foreach (var geometricObject in _objects)
        {
            if (geometricObject.IsNextTo(point, (float)precision))
            {
                return geometricObject;
            }
        }

        return null;
    }

    public Point? TryGetClosestPoint(Vector2 point, double precision = 10) => throw new NotImplementedException();
    public IFigure? TryGetClosestFigure(Vector2 point, double precision = 10) => throw new NotImplementedException();

    public void AddObject(IGeometricObject newObject)
    {
        _objects.Add(newObject);

        if (newObject is Point point)
        {
            _points.Add(point);
        }
        else if (newObject is IFigure figure)
        {
            _figures.Add(figure);

            foreach (var controlPoint in figure.ControlPoints)
            {
                _objects.Add(controlPoint);
                _points.Add(controlPoint);
            }
        }
    }

    public bool TryRemove(IGeometricObject target)
    {
        if (!_objects.Remove(target))
        {
            return false;
        }

        if (target is Point point)
        {
            return _points.Remove(point);
        }

        if (target is IFigure figure)
        {
            if (_figures.Remove(figure))
            {
                return figure.ControlPoints
                    .All(controlPoint => _objects.Remove(controlPoint) && _points.Remove(controlPoint));
            }
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
