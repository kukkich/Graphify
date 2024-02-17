using System.Drawing;
using Graphify.Geometry.Drawing;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class PolygonStyle : IStyle
{
    public Color PrimaryColor { get; set; }
    public string Name { get; set; }
    public void ApplyStyle(IDrawer drawer) => throw new NotImplementedException();
}
