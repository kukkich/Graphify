using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO;

public class JsonPointObject(uint id, Vector2 position, PointStyle style)
{
    [JsonProperty]
    public uint Id { get; internal init; } = id;
    [JsonProperty]
    public Vector2 Position { get; internal init; } = position;
    [JsonProperty]
    public PointStyle Style { get; internal init; } = style;
}
