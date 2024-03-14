using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.IO;

public class JsonFigureObject(ObjectType objectType, uint[] attachedPoint, uint[] controlPoints, IStyle style)
{
    public ObjectType ObjectType { get; internal init; } = objectType;

    public uint[] AttachedPoint { get; internal init; } = attachedPoint;

    public uint[] ControlPoints { get; internal init; } = controlPoints;

    public IStyle Style { get; internal init; } = style;
}
