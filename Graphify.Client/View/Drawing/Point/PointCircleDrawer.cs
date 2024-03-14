using System.Drawing;
using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.Point;

public class PointCircleDrawer : BaseGeometryObjectDrawer<Vector2>
{
    public PointCircleDrawer(IBaseDrawer defaultDrawer) : base(defaultDrawer) { }

    protected override void DrawDefault(Vector2 point, DrawSettings settings)
    {
        defaultDrawer.DrawFilledCircle(point, settings.PointSize, settings.PointColor);
    }

    protected override void DrawSelected(Vector2 point, DrawSettings settings)
    {
        defaultDrawer.DrawFilledCircle(point, settings.PointSize, settings.PointColor);
        defaultDrawer.DrawBorderOfCircle(point, 10, settings.PointColor, 2);
    }
}
