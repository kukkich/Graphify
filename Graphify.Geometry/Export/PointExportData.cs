using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.Export;

public class PointExportData
{
    public Vector2 Position { get; internal set; }
    public PointStyle Style { get; internal set; } = null!;

    public PointExportData(Vector2 position, PointStyle style)
    {
        Position = position;
        Style = style;
    }
}
