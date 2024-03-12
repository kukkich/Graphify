using System.Drawing;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.Drawing;

public class DrawSettings
{
    public Color LineColor { get; set; }
    public int LineThickness { get; set; }
    public Color PointColor { get; set; }
    public int PointSize { get; set; }
    public Color FillColor { get; set; }
    public ObjectState ObjectState { get; set; }
    public PointVariant PointVariant { get; set; }
}
