using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Core.Geometry;

public class ApplicationContext(IGeometryFactory factory, IGeometryContext geometryContext)
{
    public IGeometryFactory Factory { get; private set; } = factory;
    public IGeometryContext GeometryContext { get; private set; } = geometryContext;
}
