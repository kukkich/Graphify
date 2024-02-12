using System.Numerics;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Operations;

namespace Graphify.Geometry.GeometricObjects;

public class Point : IPoint
{
    public string Id => throw new NotImplementedException();
    public IFigure? AttachedTo { get; }
    public IEnumerable<IFigure> ControlPointFor { get; }
    public IEnumerable<IFigure> ControlFor => throw new NotImplementedException();

    public float X { get; }
    public float Y { get; }
    
    public Point(float x, float y)
    {
        X = x;
        Y = y;
        ControlPointFor = new List<IFigure>();
    }

    public void Update() => throw new NotImplementedException();
    public bool CanAttachTo(IFigure consumer) => throw new NotImplementedException();
    public void AttachTo(IFigure consumer) => throw new NotImplementedException();
    public bool CanApply(IOperation operation) => throw new NotImplementedException();
    public void Apply(IOperation operation) => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, out float? distance) => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
}
