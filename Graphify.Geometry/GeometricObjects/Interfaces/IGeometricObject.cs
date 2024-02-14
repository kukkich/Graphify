using System.Numerics;
using Graphify.Geometry.GeometricObjects.Operations;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometricObject : IInteractive
{
    public bool IsNextTo(Vector2 point, out float? distance);
}
