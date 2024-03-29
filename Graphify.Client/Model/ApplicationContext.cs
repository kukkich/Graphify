using System.Numerics;
using Graphify.Client.Model.Geometry;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model;

public class ApplicationContext
{
    public Surface Surface { get; private set; }
    public IEnumerable<IGeometricObject> SelectedObjects => _selectedObjects;

    private readonly IGeometryFactory _factory;

    private readonly HashSet<IGeometricObject> _selectedObjects;

    public ApplicationContext(Surface surface, IGeometryFactory factory)
    {
        Surface = surface;
        _factory = factory;
        _selectedObjects = [];
    }

    public void SetSurface(Surface newSurface)
    {
        Surface = newSurface;
    }

    public Point CreatePoint(Vector2 pointCoords)
    {
        Point newPoint = _factory.Create(pointCoords);
        Surface.AddObject(newPoint);

        return newPoint;
    }

    public IFigure CreateFigure(ObjectType type, Point[] points)
    {
        IFigure newFigure = _factory.Create(type, points);
        Surface.AddObject(newFigure);

        return newFigure;
    }

    public void AddObject(IGeometricObject geometricObject)
    {
        Surface.AddObject(geometricObject);
    }

    public IGeometricObject? Select(Vector2 position, bool clearPrevious)
    {
        var geometricObject = Surface.TryGetClosestObject(position);

        if (geometricObject is null)
        {
            ClearSelected();
            return null;
        }

        Select(geometricObject, clearPrevious);
        return geometricObject;
    }

    public void ToggleSelection(IGeometricObject geometricObject)
    {
        if (SelectedObjects.Contains(geometricObject))
        {
            UnSelect(geometricObject);
            return;
        }

        Select(geometricObject, false);
    }

    public void Select(IGeometricObject geometricObject, bool clearPrevious)
    {
        if (!Surface.Objects.Contains(geometricObject))
        {
            return;
        }

        if (clearPrevious)
        {
            ClearSelected();
        }

        if (!_selectedObjects.Add(geometricObject))
        {
            return;
        }

        geometricObject.ObjectState = ObjectState.Selected;
    }

    public void UnSelect(IGeometricObject geometricObject)
    {
        if (!Surface.Objects.Contains(geometricObject))
        {
            return;
        }

        if (!_selectedObjects.Contains(geometricObject))
        {
            return;
        }

        _selectedObjects.Remove(geometricObject);
        geometricObject.ObjectState = ObjectState.Default;
    }

    public IEnumerable<IGeometricObject> SelectAll()
    {
        ClearSelected();

        foreach (var figure in Surface.Figures.Where(x => x.CanBeMoved()))
        {
            _selectedObjects.Add(figure);
        }

        var pointsStack = new Stack<Point>();
        var handledPoints = new HashSet<Point>();
        foreach (var globalPoint in Surface.Points.Where(x => !x.IsAttached))
        {
            if (handledPoints.Contains(globalPoint))
            {
                continue;
            }

            pointsStack.Push(globalPoint);

            ResolveMoveConflicts(pointsStack, handledPoints);
        }

        foreach (var geometricObject in _selectedObjects)
        {
            geometricObject.ObjectState = ObjectState.Selected;
        }

        return _selectedObjects;
    }

    private void ResolveMoveConflicts(Stack<Point> pointsStack, HashSet<Point> handledPoints)
    {
        while (pointsStack.Count > 0)
        {
            var point = pointsStack.Pop();
            handledPoints.Add(point);

            var selectedControlFor = point.ControlFor
                .Where(_selectedObjects.Contains)
                .ToArray();

            if (!selectedControlFor.Any())
            {
                _selectedObjects.Add(point);
                continue;
            }

            if (selectedControlFor.Length == 1)
            {
                foreach (var p in selectedControlFor.First()
                             .ControlPoints
                             .Where(x => x != point &&
                                         !handledPoints.Contains(x))
                        )
                {
                    pointsStack.Push(p);
                }
            }

            if (selectedControlFor.Length > 1)
            {
                foreach (var selectedFigure in selectedControlFor)
                {
                    _selectedObjects.Remove(selectedFigure);
                    foreach (var p in selectedFigure.ControlPoints.Where(x => x != point))
                    {
                        pointsStack.Push(p);
                    }
                }
                _selectedObjects.Add(point);
            }
        }
    }

    public void ClearSelected()
    {
        foreach (var geometricObject in _selectedObjects)
        {
            geometricObject.ObjectState = ObjectState.Default;
        }

        _selectedObjects.Clear();
    }
}
