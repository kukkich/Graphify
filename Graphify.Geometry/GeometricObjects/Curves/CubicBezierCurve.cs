using System.Net.NetworkInformation;
using System.Numerics;
using Graphify.Geometry.Attachment;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Curves;

public abstract class CubicBezierCurve : ReactiveObject, IFigure, IStyled<CurveStyle>
{
    [Reactive] public CurveStyle Style { get; set; }
    [Reactive] public ObjectState ObjectState { get; set; }

    public IEnumerable<Point> Attached => _attached.Select(x => x.Object);

    public IEnumerable<Point> ControlPoints {
        get
        {
            return _points;
        }
    }

    internal bool CanBeMoved
    {
        get
        {
            foreach (var point in _points)
            {
                if (point.IsAttached)
                {
                    return false;
                }
            }
            return true;
        }
    }

    private List<AttachedPoint> _attached;

    private readonly List<Point> _points;

    /// <summary>
    /// Конструктор класса CubicBezierCurve
    /// </summary>
    /// <param name="points"> - массив опорных точек кривой</param>
    /// <param name="style"> - стиль кривой. <c>CurveStyle.Default</c>, если <c>null</c></param>
    /// <exception cref="InvalidDataException"> - исключение в случае, если размер <c>points</c> != 4</exception>
    protected CubicBezierCurve(Point[] points, CurveStyle? style = null)
    {
        if (points.Length != 4)
        {
            throw new InvalidDataException($"Невозможно создать кривую. Необходимо 4 точки, подано {points.Length}");
        }

        _points = [];
        foreach (var point in points)
        {
            _points.Add(point);
        }

        Style = style ?? CurveStyle.Default;
        _attached = [];
    }

    public void Update()
    {
        foreach (var point in _points) 
        {
            point.Update();
        }
        // TODO _attached renew (look at Circle.cs)
    }

    public void ConsumeAttach(Point attachable)
    {
        if (ControlPoints.Contains(attachable))
        {
            throw new InvalidOperationException("Нельзя присоединить точку к данной фигуре: точка является опорной для данной фигуры");
        }

        if (_attached.Find(x => x.Object == attachable) != null)
        {
            throw new InvalidOperationException("Нельзя присоединить точку к данной фигуре: точка уже присоединена к данной фигуре");
        }

        // find where to move attachable point, move and add to _attached
    }

    public void ConsumeDetach(Point attachable)
    {
        AttachedPoint? maybeAttached = _attached.Find(x => x.Object == attachable);
        if (maybeAttached != null) 
        {
            _attached.Remove(maybeAttached);
            return;
        }

        throw new InvalidOperationException("Нельзя отсоединить точку от данной фигуры: эта точка не является прикреплённой к данной фигуре");
    }

    public bool IsNextTo(Vector2 point, float distance)
    {
        if (distance <= 0)
        {
            throw new ArgumentException($"Значение distance должно быть строго положительным числом. Ожидалось: distance > 0, получено: {distance}");
        }

        // TODO fond closest distance to point
        return false;
    }
    
    public void Move(Vector2 shift)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var point in _points)
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

        foreach (var point in _points)
        {
            point.Rotate(shift, angle);
        }
    }

    public void Reflect(Point point)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var pointi in _points)
        {
            pointi.Reflect(point);
        }
    }

    public void Draw(IDrawer drawer) => throw new NotImplementedException();
    
    public FigureExportData GetExportData()
    {
        var exportData = new FigureExportData
        {
            FigureType = ObjectType.CubicBezier,
            Style = Style
        };

        // TODO find LeftBottomBound and RightTopBound

        return exportData;
    }
    public IGeometricObject Clone() => throw new NotImplementedException();
}
