using System.Numerics;
using Graphify.Client.Model.Geometry;
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

    public Point AddPoint(Vector2 pointCoords)
    {
        Point newPoint = _factory.Create(pointCoords);
        Surface.AddObject(newPoint);

        return newPoint;
    }

    public IFigure AddFigure(ObjectType type, Point[] points)
    {
        IFigure newFigure = _factory.Create(type, points);
        Surface.AddObject(newFigure);

        return newFigure;
    }

    public IGeometricObject? Select(Vector2 position, bool clearPrevious)
    {
        var geometricObject = Surface.TryGetClosestObject(position);

        if (geometricObject == null) return null;

        if (clearPrevious) _selectedObjects.Clear();

        _selectedObjects.AddLast(geometricObject);

        return geometricObject;

    }

    public IEnumerable<IGeometricObject> SelectAll()
    {
        _selectedObjects.Clear();

        foreach (var geometricObject in Surface.Objects)
        {
            _selectedObjects.AddLast(geometricObject);
        }

        return _selectedObjects;
    }
}
