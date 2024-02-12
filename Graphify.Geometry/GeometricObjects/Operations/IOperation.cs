namespace Graphify.Geometry.GeometricObjects.Operations;

public interface IOperation
{
    public bool CanBeAppliedToPoint(Point point);
    public void ApplyToPoint(Point point);

    public bool CanBeAppliedToBezierCurve(BezierCurve curve);
    public void ApplyToBezierCurve(BezierCurve curve);

    public bool CanBeAppliedToCircle(Circle circle);
    public void ApplyToCircle(Circle circle);

    public bool CanBeAppliedToPolygon(Polygon polygon);
    public void ApplyToPolygon(Polygon polygon);
}
