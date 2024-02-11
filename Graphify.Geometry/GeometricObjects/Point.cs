using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Geometry.GeometricObjects;

public class Point : IDrawable, IInteractive, IAttachable
{
    public string Id => throw new NotImplementedException();
    public IFigure? AttachedTo { get; }
    public IEnumerable<IFigure> ControlPointFor { get; }
    public IEnumerable<IFigure> ControlFor => throw new NotImplementedException();

    public void Update() => throw new NotImplementedException();

    public float X { get; }
    public float Y { get; }
    public bool CanAttachTo(IFigure consumer) => throw new NotImplementedException();

    public void AttachTo(IFigure consumer) => throw new NotImplementedException();

    

    public Point(float x, float y)
    {
        X = x;
        Y = y;
        ControlPointFor = new List<ICurve>();
    }

    public bool CanApply(IOperation operation)
    {
        return operation.CanBeAppliedToPoint(this);
    }
    public void Apply(IOperation operation)
    {
        operation.ApplyToPoint(this);
    }

    public void Draw(IDrawer drawer)
    {
        drawer.DrawPoint();
    }
}
