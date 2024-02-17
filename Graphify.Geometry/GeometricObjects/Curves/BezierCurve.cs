using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Curves;

public abstract class BezierCurve : ReactiveObject, IFigure, IStyled<CurveStyle>
{
    [Reactive] public CurveStyle Style { get; set; }
    public IEnumerable<IAttachable> Attached { get; }
    public IEnumerable<Point> ControlPoints { get; }

    protected BezierCurve()
    {
        throw new NotImplementedException();
    }

    public void Update() => throw new NotImplementedException();
    public void ConsumeAttach(IAttachable attachable) => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, float distance) => throw new NotImplementedException();
    public void Move(Vector2 shift) => throw new NotImplementedException();
    public void Rotate(System.Drawing.Point shift, float angle) => throw new NotImplementedException();
    public void Reflect(System.Drawing.Point point) => throw new NotImplementedException();

    public void Draw(IDrawer drawer) => throw new NotImplementedException();
}
