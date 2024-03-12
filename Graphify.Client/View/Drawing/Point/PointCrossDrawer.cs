using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.Point;

public class PointCrossDrawer : BaseGeometryObjectDrawer<Vector2>
{
    public PointCrossDrawer(OpenGL gl, IBaseDrawer defaultDrawer) : base(gl, defaultDrawer) { }
    protected override void DrawDefault(Vector2 point, DrawSettings settings) => throw new NotImplementedException();

    protected override void DrawSelected(Vector2 point, DrawSettings settings) => throw new NotImplementedException();
}
