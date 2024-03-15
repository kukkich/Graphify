using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.IO.Interfaces;

public interface IExporter
{
    public void Export(IGeometryContext context, string path);
}
