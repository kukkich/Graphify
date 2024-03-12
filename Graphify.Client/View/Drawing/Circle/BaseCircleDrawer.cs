using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;

namespace Graphify.Client.View.Drawing.Circle;

public class BaseCircleDrawer : BaseGeometryObjectDrawer<(Vector2, float)>
{
    public BaseCircleDrawer(IBaseDrawer defaultDrawer) : base(defaultDrawer) { }

    protected override void DrawDefault((Vector2, float) point, DrawSettings settings)
    {
        defaultDrawer.DrawCircle(point.Item1, point.Item2, settings.LineColor, settings.LineThickness);
    }

    protected override void DrawSelected((Vector2, float) point, DrawSettings settings)
    {
        defaultDrawer.DrawCircle(point.Item1, point.Item2, settings.LineColor, settings.LineThickness);
    }
}
