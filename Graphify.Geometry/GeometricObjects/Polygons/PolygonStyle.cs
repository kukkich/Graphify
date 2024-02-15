using System.Drawing;
using Graphify.Geometry.Styling;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class PolygonStyle : IStyle
{
    public Color PrimaryColor { get; set; }
    public string Name { get; set; }
}
