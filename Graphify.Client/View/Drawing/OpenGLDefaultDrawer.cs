using System.Drawing;
using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using SharpGL;

namespace Graphify.Client.View.Drawing;

public class OpenGLDefaultDrawer : IBaseDrawer
{
    private readonly OpenGL _gl;

    public OpenGLDefaultDrawer(OpenGL gl)
    {
        _gl = gl;

        _gl.Enable(OpenGL.GL_BLEND);
        _gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
        _gl.Enable(OpenGL.GL_LINE_SMOOTH);
    }

    public void DrawBezierCurve(IEnumerable<Vector2> points, Color color, int lineThickness)
    {
        _gl.LineWidth(lineThickness);
        _gl.Begin(OpenGL.GL_LINE_STRIP);

        var controlPoints = points.ToList();

        _gl.Color(color.R, color.G, color.B, color.A);

        var numPoints = 100;
        for (var i = 0; i <= numPoints; i++)
        {
            var t = (double)i / numPoints;
            double x = Math.Pow(1 - t, 3) * controlPoints[0].X +
                       3 * t * Math.Pow(1 - t, 2) * controlPoints[1].X +
                       3 * Math.Pow(t, 2) * (1 - t) * controlPoints[2].X +
                       Math.Pow(t, 3) * controlPoints[3].X;

            double y = Math.Pow(1 - t, 3) * controlPoints[0].Y +
                       3 * t * Math.Pow(1 - t, 2) * controlPoints[1].Y +
                       3 * Math.Pow(t, 2) * (1 - t) * controlPoints[2].Y +
                       Math.Pow(t, 3) * controlPoints[3].Y;
            _gl.Vertex(x, y);
        }

        _gl.End();
    }

    public void DrawBorderOfCircle(Vector2 center, float radius, Color color, int lineThickness)
    {
        _gl.LineWidth(lineThickness);
        _gl.Begin(OpenGL.GL_LINE_LOOP);

        const int numPoints = 100;
        const double angleStep = (2 * Math.PI) / numPoints;

        _gl.Color(color.R, color.G, color.B, color.A);
        for (int i = 0; i < numPoints; i++)
        {
            double angle = i * angleStep;
            double x = center.X + radius * Math.Cos(angle);
            double y = center.Y + radius * Math.Sin(angle);
            _gl.Vertex(x, y);
        }

        _gl.End();
    }

    public void DrawFilledCircle(Vector2 center, float radius, Color color)
    {
        _gl.Begin(OpenGL.GL_POLYGON);

        const int numPoints = 20;
        const double angleStep = (2 * Math.PI) / numPoints;

        _gl.Color(color.R, color.G, color.B, color.A);
        for (int i = 0; i < numPoints; i++)
        {
            double angle = i * angleStep;
            double x = center.X + radius * Math.Cos(angle);
            double y = center.Y + radius * Math.Sin(angle);
            _gl.Vertex(x, y);
        }

        _gl.End();
    }

    public void DrawPoint(Vector2 point, Color color, int pointSize)
    {
        _gl.PointSize(pointSize);
        _gl.Begin(OpenGL.GL_POINTS);
        _gl.Color(color.R, color.G, color.B, color.A);
        _gl.Vertex(point.X, point.Y);
        _gl.End();
    }

    public void DrawLine(Vector2 start, Vector2 end, Color color, int lineThickness)
    {
        _gl.LineWidth(lineThickness);
        _gl.Begin(OpenGL.GL_LINE_LOOP);
        _gl.Color(color.R, color.G, color.B, color.A);
        _gl.Vertex(start.X, start.Y);
        _gl.Vertex(end.X, end.Y);
        _gl.End();
    }

    public void DrawPolygon(IEnumerable<Vector2> points, Color color, int lineThickness)
    {
        _gl.LineWidth(lineThickness);
        _gl.Begin(OpenGL.GL_LINE_LOOP);
        _gl.Color(color.R, color.G, color.B);
        foreach (var point in points)
        {
            _gl.Vertex(point.X, point.Y);
        }
        _gl.End();
    }
}
