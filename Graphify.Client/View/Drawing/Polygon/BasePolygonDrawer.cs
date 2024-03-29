using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;

namespace Graphify.Client.View.Drawing.Polygon;

public class BasePolygonDrawer : BaseGeometryObjectDrawer<IEnumerable<Vector2>>
{
    public BasePolygonDrawer(IBaseDrawer defaultDrawer) : base(defaultDrawer) { }

    protected override void DrawDefault(IEnumerable<Vector2> point, DrawSettings settings)
    {
        defaultDrawer.DrawPolygon(point, settings.FillColor, settings.LineThickness);
    }

    protected override void DrawSelected(IEnumerable<Vector2> point, DrawSettings settings)
    {
        defaultDrawer.DrawPolygon(point, settings.FillColor, settings.LineThickness);
    }
}
