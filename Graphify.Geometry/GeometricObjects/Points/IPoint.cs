using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.Styling;

namespace Graphify.Geometry.GeometricObjects.Points;

public interface IPoint : IGeometricObject, IAttachable, IStyled<PointStyle>
{
    public float X { get; }
    public float Y { get; }
}
