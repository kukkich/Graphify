using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.BezierCurve;

public class BaseBezierCurveDrawer : BaseGeometryObjectDrawer<IEnumerable<Vector2>>
{
    public BaseBezierCurveDrawer(IBaseDrawer defaultDrawer) : base(defaultDrawer) { }

    protected override void DrawDefault(IEnumerable<Vector2> point, DrawSettings settings)
    {
        defaultDrawer.DrawBezierCurve(point, settings.LineColor, settings.LineThickness);
    }

    protected override void DrawSelected(IEnumerable<Vector2> point, DrawSettings settings)
    {
        defaultDrawer.DrawBezierCurve(point, settings.LineColor, settings.LineThickness);
    }
}
