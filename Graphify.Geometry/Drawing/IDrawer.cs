using System.Drawing;
using System.Numerics;

namespace Graphify.Geometry.Drawing;

public interface IDrawer
{
    public Color LineColor { get; set; }
    public int LineThickness { get; set; }
    public Color PointColor { get; set; }
    public int PointSize { get; set; }
    public Color FillColor { get; set; }

    public void DrawCircle(Vector2 center, float radius);
    public void DrawPoint(Vector2 point);
    public void DrawLine(Vector2 start, Vector2 end);
    public void DrawPolygon(IEnumerable<Vector2> points);
    public void DrawBezierCurve(IEnumerable<Vector2> points);
}
