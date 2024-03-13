using System.Drawing;
using System.Numerics;
using Graphify.Geometry.Drawing;

namespace Graphify.Client.View.Drawing.Base;

public interface IBaseDrawer
{
    public void DrawBorderOfCircle(Vector2 center, float radius, Color color, int lineThickness);
    public void DrawFilledCircle(Vector2 center, float radius, Color color);
    public void DrawPoint(Vector2 point, Color color, int pointSize);
    public void DrawLine(Vector2 start, Vector2 end, Color color, int lineThickness);
    public void DrawPolygon(IEnumerable<Vector2> points, Color color, int lineThickness);
    public void DrawBezierCurve(IEnumerable<Vector2> points, Color color, int lineThickness);
}
