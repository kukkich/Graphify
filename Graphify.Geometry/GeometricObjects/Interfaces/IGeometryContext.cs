using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects { get; }
    public IEnumerable<IFigure> Figures { get; }
    public IEnumerable<Point> Points { get; }

    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision); // todo если нет объектов???
    public Point? TryGetClosestPoint(Vector2 point, double precision);
    public IFigure? TryGetClosestFigure(Vector2 point, double precision);

    public bool TryRemove(IGeometricObject target);
}
