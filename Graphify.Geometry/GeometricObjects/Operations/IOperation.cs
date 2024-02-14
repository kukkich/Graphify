namespace Graphify.Geometry.GeometricObjects.Operations;

public interface IOperation<out TResult>
{
    public bool CanBeAppliedToPoint(Point point);
    public TResult ApplyToPoint(Point point);

    public bool CanBeAppliedToBezierCurve(BezierCurve curve);
    public TResult ApplyToBezierCurve(BezierCurve curve);

    public bool CanBeAppliedToCircle(Circle circle);
    public TResult ApplyToCircle(Circle circle);

    public bool CanBeAppliedToPolygon(Polygon polygon);
    public TResult ApplyToPolygon(Polygon polygon);
}
