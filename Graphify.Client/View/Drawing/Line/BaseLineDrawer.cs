using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.Line;

public class BaseLineDrawer : BaseGeometryObjectDrawer<(Vector2, Vector2)>
{
    public BaseLineDrawer(IBaseDrawer defaultDrawer) : base(defaultDrawer) { }

    protected override void DrawDefault((Vector2, Vector2) point, DrawSettings settings)
    {
        defaultDrawer.DrawLine(point.Item1, point.Item2, settings.LineColor, settings.LineThickness);
    }

    protected override void DrawSelected((Vector2, Vector2) point, DrawSettings settings)
    {
        defaultDrawer.DrawLine(point.Item1, point.Item2, settings.LineColor, settings.LineThickness);
    }
}
