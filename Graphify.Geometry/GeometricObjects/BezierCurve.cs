using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Operations;

namespace Graphify.Geometry.GeometricObjects;

public abstract class BezierCurve : IFigure
{
    public string Id => throw new NotImplementedException();
    public IEnumerable<IAttachable> Attached => throw new NotImplementedException();
    public IEnumerable<Point> ControlPoints => throw new NotImplementedException();

    private readonly Point[] _points;

    protected BezierCurve(params Point[] points)
    {
        _points = points;
    }

    public void Update() => throw new NotImplementedException();
    public bool CanApply(IOperation operation) => throw new NotImplementedException();
    public void Apply(IOperation operation) => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, out float? distance) => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
}
