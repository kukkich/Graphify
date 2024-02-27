using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.Export;

public class PointExportData
{
    public Vector2 Position { get; internal set; }
    public PointStyle Style { get; internal set; } = null!;
}
