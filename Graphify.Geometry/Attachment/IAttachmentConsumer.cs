using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.Attaching;

public interface IAttachmentConsumer : IDependencyNode
{
    public IEnumerable<IAttachable> Attached { get; }
    public IEnumerable<IPoint> ControlPoints { get; }
}
