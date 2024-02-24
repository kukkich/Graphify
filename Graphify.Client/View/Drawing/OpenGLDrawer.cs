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
        _gl.ClearColor(1.0f,1.0f,1.0f,1.0f);
        _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
    }

    public Color LineColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int LineThickness { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Color PointColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int PointSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Color FillColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void DrawBezierCurve(IEnumerable<Vector2> points) => throw new NotImplementedException();
    public void DrawCircle(Vector2 center, float radius) => throw new NotImplementedException();
    public void DrawPoint(Vector2 point) 
    {
        _gl.PointSize(5);
        _gl.Begin(OpenGL.GL_POINTS);
        _gl.Color(1f, 0f, 0f);
        _gl.Vertex(point.X, point.Y);
        _gl.End();
    }
    public void DrawPolygon(IEnumerable<Vector2> points)
    {
        _gl.PointSize(5);
        _gl.Begin(OpenGL.GL_LINE_LOOP);
        _gl.Color(0f, 0f, 0f);
        foreach (var point in points)
        {
            _gl.Vertex(point.X, point.Y);
        }
        _gl.End();
    }
}
