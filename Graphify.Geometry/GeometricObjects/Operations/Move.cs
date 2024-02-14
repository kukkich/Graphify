using System.Numerics;

namespace Graphify.Geometry.GeometricObjects.Operations;

public class Move : IOperation<Empty>
{
    public Vector2 Distance { get; }

    public Move(Vector2 distance)
    {
        Distance = distance;
    }

    public bool CanBeAppliedToPoint(Point point) => throw new NotImplementedException();
    public Empty ApplyToPoint(Point point) => throw new NotImplementedException();
    public bool CanBeAppliedToBezierCurve(BezierCurve curve) => throw new NotImplementedException();
    public Empty ApplyToBezierCurve(BezierCurve curve) => throw new NotImplementedException();
    public bool CanBeAppliedToCircle(Circle circle) => throw new NotImplementedException();
    public Empty ApplyToCircle(Circle circle) => throw new NotImplementedException();
    public bool CanBeAppliedToPolygon(Polygon polygon) => throw new NotImplementedException();
    public Empty ApplyToPolygon(Polygon polygon) => throw new NotImplementedException();
}
