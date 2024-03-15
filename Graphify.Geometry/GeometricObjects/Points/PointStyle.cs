using System.Drawing;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Curves;
using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Points;

public class PointStyle : CurveStyle
{
    [Reactive, JsonProperty] public PointVariant Variant { get; set; }

    public static new PointStyle Default => new(CurveStyle.Default, PointVariant.Circle, DefaultSize);

    public PointStyle(CurveStyle curveStyle, PointVariant variant, int size) : base(curveStyle?.PrimaryColor ?? Color.Black, curveStyle?.Name ?? "Default", size)
    {
        Variant = variant;
    }

    public override void ApplyStyle(IDrawer drawer)
    {
        drawer.Settings.PointColor = PrimaryColor;
        drawer.Settings.PointSize = Size;
    }
}
