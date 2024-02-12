using System.Numerics;
using Graphify.Geometry.GeometricObjects.Operations;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometricObject : IDrawable, IInteractive, IImportable
{
    public bool IsNextTo(Vector2 point, out float? distance);
}
