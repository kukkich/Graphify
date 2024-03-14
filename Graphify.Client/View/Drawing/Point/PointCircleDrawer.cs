using System.Drawing;
using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;

namespace Graphify.Client.View.Drawing.Point;

public class PointCircleDrawer : BasePointDrawer
{
    public PointCircleDrawer(IBaseDrawer defaultDrawer) : base(defaultDrawer) { }

    protected override void DrawDefault(Vector2 point, DrawSettings settings)
    {
        defaultDrawer.DrawFilledCircle(point, settings.PointSize, settings.PointColor);
    }

    protected override void DrawSelected(Vector2 point, DrawSettings settings)
    {
        defaultDrawer.DrawPoint(point, settings.PointColor, settings.PointSize);
        defaultDrawer.DrawFilledCircle(point, settings.PointSize, settings.PointColor);
        defaultDrawer.DrawBorderOfCircle(point, 10, settings.PointColor, 2);
    }

    protected override void DrawControlPoint(Vector2 point, DrawSettings settings)
    {
        defaultDrawer.DrawPoint(point, Color.Aqua, settings.PointSize);
    }
}
