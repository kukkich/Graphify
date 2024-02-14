using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Geometry.Attaching;

public interface IAttachmentConsumer : IDependencyNode
{
    public IEnumerable<IAttachable> Attached { get; }
    public IEnumerable<IPoint> ControlPoints { get; }
}
