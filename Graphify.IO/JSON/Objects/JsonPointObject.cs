using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO.JSON.Objects;

public class JsonPointObject(uint id, Vector2 position, PointStyle style)
{
    public uint Id { get; internal init; } = id;

    public Vector2 Position { get; internal init; } = position;
    public PointStyle Style { get; internal init; } = style;
}
