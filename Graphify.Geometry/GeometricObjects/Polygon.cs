using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Operations;

namespace Graphify.Geometry.GeometricObjects;

public class Polygon : IFigure
{
    public string Id { get; }
    public IEnumerable<IAttachable> Attached { get; }
    public IEnumerable<IPoint> ControlPoints { get; }
    
    private readonly IPoint[] _points;

    public Polygon(params IPoint[] points)
    {
        _points = points;
    }

    public void Update() => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, out float? distance) => throw new NotImplementedException();
    public bool CanApply<T>(IOperation<T> operation) => throw new NotImplementedException();
    public T Apply<T>(IOperation<T> operation) => throw new NotImplementedException();
}
