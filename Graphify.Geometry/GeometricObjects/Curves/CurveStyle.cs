using System.Drawing;
using Graphify.Geometry.Drawing;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Curves;

public class CurveStyle : ReactiveObject, IStyle
{
    [Reactive] public Color PrimaryColor { get; set; }
    [Reactive] public string Name { get; set; }
    [Reactive] public int Size { get; set; }
    public static CurveStyle Default => new(Color.Black, "Default", CurveStyle.DefaultSize);

    public static int DefaultSize { get; set; } = 4;

    public CurveStyle(Color primary, string name, int size)
    {
        PrimaryColor = primary;
        Name = name;
        Size = size;
    }

    public virtual void ApplyStyle(IDrawer drawer) {
        drawer.Settings.LineColor = PrimaryColor;
        drawer.Settings.LineThickness = Size;
    }
}
