using Graphify.Geometry.Attaching;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IPoint : IGeometricObject, IAttachable
{
    public float X { get; }
    public float Y { get; }
}
