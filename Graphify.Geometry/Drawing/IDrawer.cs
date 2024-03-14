using System.Drawing;
using System.Numerics;

namespace Graphify.Geometry.Drawing;

public interface IDrawer
{
    public DrawSettings Settings { get; }

    public void Reset();
    public void DrawCircle(Vector2 center, float radius, ObjectState state);
    public void DrawPoint(Vector2 point, ObjectState state);
    public void DrawLine(Vector2 start, Vector2 end, ObjectState state);
    public void DrawPolygon(IEnumerable<Vector2> points, ObjectState state);
    public void DrawBezierCurve(IEnumerable<Vector2> points, ObjectState state);
}
