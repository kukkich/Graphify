using System.Numerics;
using System.Windows.Navigation;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Geometry;

public class Surface : IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects => _figures.Union<IGeometricObject>(_points);
    public IEnumerable<IFigure> Figures => _figures;
    public IEnumerable<Point> Points => _points;

    private readonly HashSet<IFigure> _figures = [];
    private readonly HashSet<Point> _points = [];

    // TODO add object filter
    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision = 10)
    {
        var closestPoint = TryGetClosestPoint(point, precision);

        if (closestPoint != null)
        {
            return closestPoint;
        }

        var closestFigure = TryGetClosestFigure(point, precision);

        if (closestFigure != null)
        {
            return closestFigure;
        }
        
        return null;
    }

    public Point? TryGetClosestPoint(Vector2 point, double precision = 10)
    {
        var closestPoint = _points.FirstOrDefault(p => p.IsNextTo(point, (float)precision));

        return closestPoint;
    }

    public IFigure? TryGetClosestFigure(Vector2 point, double precision = 10)
    {
        var closestFigure = _figures.FirstOrDefault(f => f.IsNextTo(point, (float)precision));

        return closestFigure;
    }

    public void AddObject(IGeometricObject newObject)
    {
        if (newObject is Point point)
        {
            _points.Add(point);
        }
        else if (newObject is IFigure figure)
        {
            _figures.Add(figure);

            foreach (var controlPoint in figure.ControlPoints)
            {
                _points.Add(controlPoint);
            }
        }
    }

    public bool TryRemove(IGeometricObject target)
    {
        //TODO removing
        if (target is Point point)
        {
            return TryRemovePoint(point);
        }
        if (target is IFigure figure)
        {
            if (_figures.Remove(figure))
            {
                foreach (var controlPoint in figure.ControlPoints)
                {
                    return TryRemovePoint(controlPoint);
                }
            }
        }

        throw new ArgumentException("Target object not found");
    }

    public void Clear()
    {
        _figures.Clear();
        _points.Clear();
    }

    private bool TryRemovePoint(Point point)
    {
        if (point.ControlFor.Any(figureOfPoint => !_figures.Remove(figureOfPoint)))
        {
            return false;
        }

        var figureAttached = point.AttachedTo;

        figureAttached?.ConsumeDetach(point);

        return _points.Remove(point);
    }
}
