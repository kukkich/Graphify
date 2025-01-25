using System.Numerics;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Geometry;

public class Surface : IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects => _figures.Union<IGeometricObject>(_points);
    public IEnumerable<IFigure> Figures => _figures;
    public IEnumerable<Point> Points => _points;

    public delegate void OnGeometryObjectAdded(IGeometricObject newObject);
    public event OnGeometryObjectAdded OnGeometryObjectAddedEvent = null!;

    public delegate void OnGeometryObjectRemoved(IGeometricObject newObject);
    public event OnGeometryObjectRemoved OnGeometryObjectRemovedEvent = null!;

    private readonly HashSet<IFigure> _figures = [];
    private readonly HashSet<Point> _points = [];

    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision = 15)
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

    public Point? TryGetClosestPoint(Vector2 point, double precision = 15)
    {
        var closestPoint = _points.FirstOrDefault(p => p.IsNextTo(point, (float)precision));

        return closestPoint;
    }

    public IFigure? TryGetClosestFigure(Vector2 point, double precision = 15)
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
                controlPoint.AssignControl(figure);
            }
        }

        OnGeometryObjectAddedEvent.Invoke(newObject);
    }

    public bool TryRemove(IGeometricObject target)
    {
        //TODO removing
        if (target is Point point)
        {
            if (TryRemovePoint(point))
            {
                OnGeometryObjectRemovedEvent.Invoke(target);
                return true;
            }

            return false;
        }
        if (target is IFigure figure)
        {
            foreach (var controlPoint in figure.ControlPoints)
            {

                OnGeometryObjectRemovedEvent.Invoke(target);
                if (TryRemovePoint(controlPoint) == false) return false;

                // TODO Were there during conflict resolving, remove if its not necessary

                //OnGeometryObjectRemovedEvent.Invoke(target);
                //foreach (var controlPoint in figure.ControlPoints)
                //{
                //    return TryRemovePoint(controlPoint);
                //}
            }

            return _figures.Remove(figure);
        }

        throw new ArgumentException("Target object not found");
    }

    public void CancelObject(IGeometricObject geometricObject)
    {
        if (geometricObject is Point point)
        {
            _points.Remove(point);
        }
        else if (geometricObject is IFigure figure)
        {
            _figures.Remove(figure);

            foreach (var controlPoint in figure.ControlPoints)
            {
                controlPoint.RetrieveControl(figure);

                if (controlPoint.ControlFor.Any())
                    continue;

                _points.Remove(controlPoint);
            }
        }
    }

    public void Clear()
    {
        _figures.Clear();
        _points.Clear();
    }

    private bool TryRemovePoint(Point point)
    {
        var controlledFigures = point.ControlFor.ToArray();

        foreach (var figureOfPoint in controlledFigures)
        {
            if (_figures.Contains(figureOfPoint))
            {
                if (!_figures.Remove(figureOfPoint))
                {
                    return false;
                }
            }

            point.RetrieveControl(figureOfPoint);
        }

        var figureAttached = point.AttachedTo;

        figureAttached?.ConsumeDetach(point);

        return _points.Remove(point);
    }
}
