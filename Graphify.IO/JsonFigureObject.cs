using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Newtonsoft.Json;
using static Graphify.IO.Importers.GraphifyImporter;

namespace Graphify.IO;

public class JsonFigureObject(ObjectType objectType, uint[] attachedPoint, uint[] controlPoints, IStyle style)
{
    public ObjectType ObjectType { get; internal init; } = objectType;

    public uint[] AttachedPoint { get; internal init; } = attachedPoint;

    public uint[] ControlPoints { get; internal init; } = controlPoints;
    [JsonConverter(typeof(StyleConverter))]
    public IStyle Style { get; internal init; } = style;
}
