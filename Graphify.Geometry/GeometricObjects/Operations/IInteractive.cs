namespace Graphify.Geometry.GeometricObjects.Operations;

public interface IInteractive
{
    public bool CanApply(IOperation operation);
    public void Apply(IOperation operation);
}
