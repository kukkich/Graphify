namespace Graphify.Geometry.GeometricObjects.Interfaces.Public;

public interface IImporter
{
    public void ImportPoint(Point point);
    public void ImportBezierCurve(BezierCurve curve);
    public void ImportCircle(Circle circle);
    public void ImportPolyGon(Polygon polygon);
}
