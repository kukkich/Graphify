using System.Drawing;
using Graphify.Geometry.Drawing;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class PolygonStyle : ReactiveObject, IStyle
{
    [Reactive, JsonProperty] public Color PrimaryColor { get; set; }
    [Reactive, JsonProperty] public string Name { get; set; }
    [Reactive, JsonProperty] public bool Visible { get; set; } = true;
    public static int DefaultSize { get; set; } = 4;

    public static PolygonStyle Default => new(Color.FromArgb(120, 255, 120, 120), "Default");

    public PolygonStyle(Color primary, string name)
    {
        PrimaryColor = primary;
        Name = name;
    }

    public void ApplyStyle(IDrawer drawer)
    {
        drawer.Settings.FillColor = PrimaryColor;
    }
}
