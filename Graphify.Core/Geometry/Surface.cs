using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Core.Geometry;

public class Surface(IGeometryContext geometryContext)
{
    public IGeometryContext GeometryContext { get; private set; } = geometryContext;

    public void Clear()
    {
        GeometryContext.Clear();
    }
}
