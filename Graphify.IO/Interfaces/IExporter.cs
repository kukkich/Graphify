using System.Numerics;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO.Interfaces;

public interface IExporter
{
    public void Export(IGeometryContext context, string path);
}
