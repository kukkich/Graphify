using System.Drawing;
using Graphify.Geometry.Drawing;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class PolygonStyle : ReactiveObject, IStyle
{
    [Reactive] public Color PrimaryColor { get; set; }
    [Reactive] public string Name { get; set; }
    public void ApplyStyle(IDrawer drawer) => throw new NotImplementedException();
}
