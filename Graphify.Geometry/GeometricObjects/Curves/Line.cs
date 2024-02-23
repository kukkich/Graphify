using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Curves;

public class Line : ReactiveObject, IFigure, IStyled<CurveStyle>
{
    /// <summary>
    /// Список точек, прикреплённых к данному объекту
    /// </summary>
    public IEnumerable<IAttachable> Attached { get => _attached; }

    /// <summary>
    /// Контрольные точки фигуры, по которым она строится
    /// TODO: А как собственно опорные точки вообще задаются?
    /// </summary>
    public IEnumerable<Point> ControlPoints { get; }

    /// <summary>
    /// Стиль прямой
    /// </summary>
    [Reactive] public CurveStyle Style { get; set; }


    private List<IAttachable> _attached;


    public Line() 
    {
        Style = CurveStyle.Default;
        _attached = new List<IAttachable>();

    }


    public void ConsumeAttach(IAttachable attachable) => throw new NotImplementedException();
    public void Update() => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, float distance) => throw new NotImplementedException();
    public void Move(Vector2 shift) => throw new NotImplementedException();
    public void Rotate(System.Drawing.Point shift, float angle) => throw new NotImplementedException();
    public void Reflect(System.Drawing.Point point) => throw new NotImplementedException();
}
