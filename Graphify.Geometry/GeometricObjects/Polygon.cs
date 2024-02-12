using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Operations;

namespace Graphify.Geometry.GeometricObjects;

public class Polygon : IFigure
{
    public string Id { get; }
    public IEnumerable<IAttachable> Attached { get; }
    public IEnumerable<Point> ControlPoints { get; }
    
    private readonly Point[] _points;

    public Polygon(params Point[] points)
    {
        _points = points;
    }

    public void Update() => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
    public bool CanApply(IOperation operation) => throw new NotImplementedException();
    public void Apply(IOperation operation) => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, out float? distance) => throw new NotImplementedException();
}
