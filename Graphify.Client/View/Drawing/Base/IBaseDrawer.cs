using System.Numerics;
using Graphify.Geometry.Drawing;

namespace Graphify.Client.View.Drawing.Base;

public interface IBaseDrawer
{
    public void DrawCircle(Vector2 center, float radius, DrawSettings drawSettings);
    public void DrawPoint(Vector2 point, DrawSettings drawSettings);
    public void DrawLine(Vector2 start, Vector2 end, DrawSettings drawSettings);
    public void DrawPolygon(IEnumerable<Vector2> points, DrawSettings drawSettings);
    public void DrawBezierCurve(IEnumerable<Vector2> points, DrawSettings drawSettings);
}
