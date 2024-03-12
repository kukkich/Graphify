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
        defaultDrawer.DrawPoint(point, settings.PointColor, settings.PointSize);
    }

    protected override void DrawSelected(Vector2 point, DrawSettings settings)
    {
        defaultDrawer.DrawCircle(point, 10, settings.LineColor, 2);
        defaultDrawer.DrawPoint(point, settings.PointColor, settings.PointSize);
    }
}
