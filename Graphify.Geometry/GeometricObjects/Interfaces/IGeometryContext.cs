using System.Numerics;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects { get; }
    public IEnumerable<IFigure> Figures { get; }
    public IEnumerable<IPoint> Points { get; set; }
    public IGeometricObject GetClosestTo(Vector2 point, double precision);
    public void TryRemove(IGeometricObject target);
}
