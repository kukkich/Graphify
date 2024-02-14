namespace Graphify.Geometry.GeometricObjects.Operations;

public interface IInteractive
{
    public bool CanApply<T>(IOperation<T> operation);
    public T Apply<T>(IOperation<T> operation);
}
