using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Attachment;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.Export;
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

    private List<AttachedPoint> _attached; //TODO: подумать над переходом на HashSet или любой другой *Set

    private Point _pointA;
    private Point _pointB;

    public Circle(Point A, Point B, CurveStyle? style = null)
    {
        _pointA = A;
        _pointB = B;

        Style = style ?? CurveStyle.Default;
        _attached = [];
    }

    public void Update() => throw new NotImplementedException();
    public void ConsumeAttach(Point attachable) => throw new NotImplementedException();
    public void ConsumeDetach(Point attachable) => throw new NotImplementedException ();
    public bool IsNextTo(Vector2 point, float distance) => throw new NotImplementedException();
    public void Move(Vector2 shift) => throw new NotImplementedException();
    public void Rotate(Point shift, float angle) => throw new NotImplementedException();
    public void Reflect(Point point) => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
    public FigureExportData GetExportData() => throw new NotImplementedException();
}
