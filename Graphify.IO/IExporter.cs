using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.IO;

public interface IExporter
{
    public void Export(IGeometryContext context, string path);
}
