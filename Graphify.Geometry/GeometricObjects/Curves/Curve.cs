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

public abstract class Curve : ReactiveObject, IFigure, IStyled<CurveStyle>
{
    public abstract int RequiredPointsCount { get; }

    [Reactive] public CurveStyle Style { get; set; }
    [Reactive] public ObjectState ObjectState { get; set; }
    IEnumerable<AttachedPoint> IAttachmentConsumer.Attached => Attached;
    public IEnumerable<Point> ControlPoints => Points;

    internal bool CanBeMoved => Points.All(point => !point.IsAttached);
    bool IGeometricObject.CanBeMoved() => CanBeMoved;

    protected readonly List<AttachedPoint> Attached;
    protected readonly Point[] Points;
    
    protected Curve(Point[] points, CurveStyle? style = null)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        if (points.Length != RequiredPointsCount)
        {
            throw new InvalidDataException($"Невозможно создать кривую. Необходимо {RequiredPointsCount} точки, подано {points.Length}");
        }

        Points = points.ToArray();

        Style = style ?? CurveStyle.Default;
        Attached = [];
    }
    
    public abstract void Attach(Point attachable);
    public void Detach(Point attachable)
    {
        AttachedPoint? maybeAttached = Attached.Find(x => x.Object == attachable);
        if (maybeAttached is null)
        {
            throw new InvalidOperationException(
                "Нельзя отсоединить точку от данной фигуры: эта точка не является прикреплённой к данной фигуре"
            );
        }
        Attached.Remove(maybeAttached);
    }
    
    public abstract bool IsNextTo(Vector2 point, float distance);
    public abstract void Update();
    public void Move(Vector2 shift)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var point in Points)
        {
            point.Move(shift);
        }
    }
    public void Rotate(Point shift, float angle)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var objPoint in ControlPoints)
        {
            objPoint.Rotate(shift, angle);
        }
    }
    public void Reflect(Point point)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var pointi in Points)
        {
            pointi.Reflect(point);
        }
    }
    
    public abstract void Draw(IDrawer drawer);
    
    public abstract IGeometricObject Clone();
    public abstract FigureExportData GetExportData();
}
