﻿using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing;

public class OpenGLDefaultDrawer : IBaseDrawer
{
    private readonly OpenGL _gl;
    
    public OpenGLDefaultDrawer(OpenGL gl)
    {
        _gl = gl;
    }
    
    public void DrawBezierCurve(IEnumerable<Vector2> points, DrawSettings settings)
    {
        _gl.PointSize(1);
        _gl.Begin(OpenGL.GL_LINE_STRIP);

        var controlPoints = points.ToList();

        _gl.Color(0f, 0f, 0f);
        for (double t = 0; t <= 1; t += 0.01)
        {
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

    public void DrawCircle(Vector2 center, float radius, DrawSettings settings)
    {
        _gl.PointSize(1);
        _gl.Begin(OpenGL.GL_POINTS);

        const int numPoints = 1000;
        const double angleStep = (2 * Math.PI) / numPoints;

        _gl.Color(0f, 0f, 0f);
        for (int i = 0; i < numPoints; i++)
        {
            double angle = i * angleStep;
            double x = center.X + radius * Math.Cos(angle);
            double y = center.Y + radius * Math.Sin(angle);
            _gl.Vertex(x, y);
        }

        _gl.End();
    }

    public void DrawPoint(Vector2 point, DrawSettings settings)
    {
        _gl.PointSize(5);
        _gl.Begin(OpenGL.GL_POINTS);
        _gl.Color(1f, 0f, 0f);
        _gl.Vertex(point.X, point.Y);
        _gl.End();
    }

    public void DrawLine(Vector2 start, Vector2 end, DrawSettings settings)
    {
    }

    public void DrawPolygon(IEnumerable<Vector2> points, DrawSettings settings)
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
