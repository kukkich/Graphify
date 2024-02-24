using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Curves;

public class Circle : ReactiveObject, IFigure, IStyled<CurveStyle>
{
    public IEnumerable<Point> Attached { get; }
    public IEnumerable<Point> ControlPoints { get; }
    [Reactive] public CurveStyle Style { get; set; }

    public Circle()
    {
        throw new NotImplementedException();
    }

    public void Update() => throw new NotImplementedException();
    public void ConsumeAttach(Point attachable) => throw new NotImplementedException();
    public void ConsumeDetach(Point attachable) => throw new NotImplementedException ();
    public bool IsNextTo(Vector2 point, float distance) => throw new NotImplementedException();
    public void Move(Vector2 shift) => throw new NotImplementedException();
    public void Rotate(Point shift, float angle) => throw new NotImplementedException();
    public void Reflect(Point point) => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
}
