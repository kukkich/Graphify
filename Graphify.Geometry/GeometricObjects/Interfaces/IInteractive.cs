namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IInteractive
{
    public bool CanApply(IOperation operation);
    public void Apply(IOperation operation);
}

public interface IOperation
{
    public bool CanBeAppliedToPoint(Point point);
    public void ApplyToPoint(Point point);

    public bool CanBeAppliedToCurve(BezierCurve curve);
    public void ApplyToCurve(BezierCurve curve);
}
