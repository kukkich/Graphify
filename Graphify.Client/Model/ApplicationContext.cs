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

    public delegate void OnSurfaceChanged(Surface newSurface);
    public event OnSurfaceChanged OnSurfaceChangedEvent;

    private readonly IGeometryFactory _factory;

    private readonly LinkedList<IGeometricObject> _selectedObjects;

    public ApplicationContext(Surface surface, IGeometryFactory factory)
    {
        Surface = surface;
        _factory = factory;
        _selectedObjects = new LinkedList<IGeometricObject>();

        OnSurfaceChangedEvent?.Invoke(surface);
    }

    public void SetSurface(Surface newSurface)
    {
        Surface = newSurface;
        OnSurfaceChangedEvent?.Invoke(newSurface);
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

        if (_selectedObjects.Contains(geometricObject))
        {
            return;
        }

        _selectedObjects.AddLast(geometricObject);
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

        foreach (var geometricObject in Surface.Objects)
        {
            _selectedObjects.AddLast(geometricObject);
            geometricObject.ObjectState = ObjectState.Selected;
        }

        return _selectedObjects;
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
