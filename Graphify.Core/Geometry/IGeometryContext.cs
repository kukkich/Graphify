using System.Numerics;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Core.Geometry;

public interface IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects { get; }
    public IEnumerable<IFigure> Figures { get; }
    public IEnumerable<Point> Points { get; }
    
    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision); // todo если нет объектов???
    public Point? TryGetClosestPoint(Vector2 point, double precision);
    public IFigure? TryGetClosestFigure(Vector2 point, double precision);

    public bool TryRemove(IGeometricObject target);
    public void Clear();
}
