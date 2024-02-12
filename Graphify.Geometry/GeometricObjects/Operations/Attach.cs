namespace Graphify.Geometry.GeometricObjects.Operations;

public class Attach : IOperation
{
    public bool CanBeAppliedToPoint(Point point) => throw new NotImplementedException();

    public void ApplyToPoint(Point point) => throw new NotImplementedException();

    public bool CanBeAppliedToBezierCurve(BezierCurve curve) => throw new NotImplementedException();

    public void ApplyToBezierCurve(BezierCurve curve) => throw new NotImplementedException();

    public bool CanBeAppliedToCircle(Circle circle) => throw new NotImplementedException();

    public void ApplyToCircle(Circle circle) => throw new NotImplementedException();
    public bool CanBeAppliedToPolygon(Polygon polygon) => throw new NotImplementedException();

    public void ApplyToPolygon(Polygon polygon) => throw new NotImplementedException();
}
