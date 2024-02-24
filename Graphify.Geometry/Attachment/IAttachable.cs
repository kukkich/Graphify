using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Geometry.Attaching;

public interface IAttachable : IDependencyNode
{
    public bool CanAttachTo(IFigure consumer);
    public void AttachTo(IFigure consumer);
    public void Detach();
    public IFigure? AttachedTo { get; }
    public bool IsAttached { get; }
    public IEnumerable<IFigure> ControlFor { get; }
}
