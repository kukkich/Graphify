using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Core.Geometry;

public class Surface(IGeometryContext geometryContextData)
{
    public IGeometryContext GeometryContext { get; private set; } = geometryContextData;
}
