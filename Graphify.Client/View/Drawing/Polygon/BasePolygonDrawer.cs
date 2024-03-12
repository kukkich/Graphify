using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.Polygon;

public class BasePolygonDrawer : BaseGeometryObjectDrawer<IEnumerable<Vector2>>
{
    public BasePolygonDrawer(OpenGL gl, IBaseDrawer defaultDrawer) : base(gl, defaultDrawer) { }

    protected override void DrawDefault(IEnumerable<Vector2> point, DrawSettings settings)
    {
    }

    protected override void DrawSelected(IEnumerable<Vector2> point, DrawSettings settings)
    {
        
    }
}
