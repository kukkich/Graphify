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

    public static int DefaultSize { get; set; } = 4;

    public CurveStyle(Color primary, string name, int size)
    {
        PrimaryColor = primary;
        Name = name;
        Size = size;
    }

    public void ApplyStyle(IDrawer drawer) => throw new NotImplementedException();

    public static CurveStyle Default => new(Color.Black, "Default", CurveStyle.DefaultSize);
}
