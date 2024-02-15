using System.Drawing;
using Graphify.Geometry.Styling;

namespace Graphify.Geometry.GeometricObjects.Curves;

public class CurveStyle : IStyle
{
    public Color PrimaryColor { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
}
