using System.Drawing;
using System.Numerics;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing;

public class OpenGLDrawer : IDrawer
{
    private OpenGL _gl;

    public void InitGl(OpenGL gl)
    {
        _gl = gl;
    }

    public Color LineColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int LineThickness { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Color PointColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int PointSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Color FillColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void DrawBezierCurve(IEnumerable<Vector2> points) => throw new NotImplementedException();
    public void DrawCircle(Vector2 center, float radius) => throw new NotImplementedException();
    public void DrawPoint(Vector2 point) => throw new NotImplementedException();
    public void DrawPolygon(IEnumerable<Vector2> points) => throw new NotImplementedException();
}
