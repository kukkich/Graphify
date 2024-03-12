using System.Drawing;
using Graphify.Geometry.Drawing;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class PolygonStyle : ReactiveObject, IStyle
{
    [Reactive] public Color PrimaryColor { get; set; }
    [Reactive] public Color LineColor { get; set; }
    [Reactive] public string Name { get; set; }
    [Reactive] public int Size { get; set; }
    public static int DefaultSize { get; set; } = 4;

    public static PolygonStyle Default => new(Color.Orange, Color.Black, "Default", DefaultSize);

    public PolygonStyle(Color primary, Color lineColor, string name, int size)
    {
        PrimaryColor = primary;
        LineColor = lineColor;
        Name = name;
        Size = size;
    }

    public void ApplyStyle(IDrawer drawer)
    {
        drawer.Settings.LineColor = LineColor;
        drawer.Settings.FillColor = PrimaryColor;
        drawer.Settings.LineThickness = Size;
    }
}
