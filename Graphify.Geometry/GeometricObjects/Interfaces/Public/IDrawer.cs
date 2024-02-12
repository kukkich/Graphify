namespace Graphify.Geometry.GeometricObjects.Interfaces.Public;

public interface IDrawer
{
    public void DrawPoint(Point point);
    public void DrawBezierCurve(BezierCurve curve);
    public void DrawCircle(Circle circle);
    public void DrawLine();
}
