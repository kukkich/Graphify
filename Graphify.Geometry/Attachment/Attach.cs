using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Operations;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Polygons;

namespace Graphify.Geometry.Attachment;

public class Attach : IOperation<Empty>
{
    public bool CanBeAppliedToPoint(Point point) => throw new NotImplementedException();
    public Empty ApplyToPoint(Point point) => throw new NotImplementedException();
    public bool CanBeAppliedToBezierCurve(BezierCurve curve) => throw new NotImplementedException();
    public Empty ApplyToBezierCurve(BezierCurve curve) => throw new NotImplementedException();
    public bool CanBeAppliedToCircle(Circle circle) => throw new NotImplementedException();
    public Empty ApplyToCircle(Circle circle) => throw new NotImplementedException();
    public bool CanBeAppliedToPolygon(Polygon polygon) => throw new NotImplementedException();
    public Empty ApplyToPolygon(Polygon polygon) => throw new NotImplementedException();
}
