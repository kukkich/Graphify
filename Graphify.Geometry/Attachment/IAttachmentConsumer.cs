using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.Attaching;

public interface IAttachmentConsumer : IDependencyNode
{
    public IEnumerable<Point> Attached { get; }
    public IEnumerable<Point> ControlPoints { get; }
    public void ConsumeAttach(Point attachable);
    public void ConsumeDetach(Point attachable);
}
