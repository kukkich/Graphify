using Graphify.Geometry.Attachment;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.Attaching;

public interface IAttachmentConsumer : IDependencyNode
{
    public IEnumerable<AttachedPoint> Attached { get; }
    public IEnumerable<Point> ControlPoints { get; }
    public void Attach(Point attachable);
    public void Detach(Point attachable);
}
