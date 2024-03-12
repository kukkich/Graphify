using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.Line;

public class BaseLineDrawer : BaseGeometryObjectDrawer<(Vector2, Vector2)>
{
    public BaseLineDrawer(OpenGL gl, IBaseDrawer defaultDrawer) : base(gl, defaultDrawer) { }
    protected override void DrawDefault((Vector2, Vector2) point, DrawSettings settings) => throw new NotImplementedException();

    protected override void DrawSelected((Vector2, Vector2) point, DrawSettings settings) => throw new NotImplementedException();
}
