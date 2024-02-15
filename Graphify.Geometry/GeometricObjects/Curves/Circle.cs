using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Operations;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.Styling;

namespace Graphify.Geometry.GeometricObjects.Curves;

public class Circle : IFigure, IStyled<CurveStyle>
{
    public string Id => throw new NotImplementedException();
    public IEnumerable<IAttachable> Attached => throw new NotImplementedException();
    public IEnumerable<IPoint> ControlPoints => throw new NotImplementedException();
    public CurveStyle Style { get; set; }

    public float CenterX { get; }
    public float CenterY { get; }
    public float Radius { get; }

    public Circle(float centerX, float centerY, float radius)
    {
        CenterX = centerX;
        CenterY = centerY;
        Radius = radius;
    }

    public void Update() => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, out float? distance) => throw new NotImplementedException();
    public bool CanApply<T>(IOperation<T> operation) => throw new NotImplementedException();
    public T Apply<T>(IOperation<T> operation) => throw new NotImplementedException();
}
