using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.Drawing;
using Newtonsoft.Json;
using Graphify.IO.Importers;

namespace Graphify.IO;

public class JsonFigureObject(ObjectType objectType, uint[] attachedPoint, uint[] controlPoints, IStyle style)
{
    public ObjectType ObjectType { get; internal init; } = objectType;

    public uint[] AttachedPoint { get; internal init; } = attachedPoint;

    public uint[] ControlPoints { get; internal init; } = controlPoints;

    [JsonConverter(typeof(StyleConverter))]
    public IStyle Style { get; internal init; } = style;
}
