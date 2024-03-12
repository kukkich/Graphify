using System.Numerics;
using Graphify.Client.Model.Geometry;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model;

public class ApplicationContext
{
    public Surface Surface { get; private set; }
    public LinkedList<IGeometricObject> SelectedObjects { get; private set; }

    public delegate void OnSurfaceChanged(Surface newSurface);
    public event OnSurfaceChanged OnSurfaceChangedEvent;

    private readonly IGeometryFactory _factory;

    public ApplicationContext(Surface surface, IGeometryFactory factory)
    {
        Surface = surface;
        _factory = factory;

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
        Surface.AddPoint(newPoint);

        return newPoint;
    }

    public IFigure AddFigure(ObjectType type, Point[] points)
    {
        IFigure newFigure = _factory.Create(type, points);
        Surface.AddFigure(newFigure);

        return newFigure;
    }
}
