using System.Drawing;
using Graphify.Geometry.Drawing;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class PolygonStyle : ReactiveObject, IStyle
{
    [Reactive, JsonProperty] public Color PrimaryColor { get; set; }
    [Reactive, JsonProperty] public Color LineColor { get; set; }
    [Reactive, JsonProperty] public string Name { get; set; }
    [Reactive, JsonProperty] public int Size { get; set; }
    public static int DefaultSize { get; set; } = 4;

    public static PolygonStyle Default => new(Color.FromArgb(0, 255, 255, 255), Color.Black, "Default", DefaultSize);

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
