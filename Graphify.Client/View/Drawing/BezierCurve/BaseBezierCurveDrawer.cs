using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.BezierCurve;

public class BaseBezierCurveDrawer : BaseGeometryObjectDrawer<IEnumerable<Vector2>>
{
    public BaseBezierCurveDrawer(OpenGL gl, IBaseDrawer defaultDrawer) : base(gl, defaultDrawer) { }
    protected override void DrawDefault(IEnumerable<Vector2> point, DrawSettings settings) => throw new NotImplementedException();

    protected override void DrawSelected(IEnumerable<Vector2> point, DrawSettings settings) => throw new NotImplementedException();
}
