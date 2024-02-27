using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometryContextData
{
    public IEnumerable<IGeometricObject> Objects { get; }
    public IEnumerable<IFigure> Figures { get; }
    public IEnumerable<Point> Points { get; }
}
