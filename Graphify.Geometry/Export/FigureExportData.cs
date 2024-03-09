using System.Numerics;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Geometry.Export;

public class FigureExportData
{
    public ObjectType FigureType { get; internal set; }
    public Vector2 LeftBottomBound { get; internal set; }
    public Vector2 RightTopBound { get; internal set; }
    public IStyle Style { get; internal set; } = null!;
}
