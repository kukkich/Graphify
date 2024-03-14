using System.Net.NetworkInformation;
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

public abstract class CubicBezierCurve : ReactiveObject, IFigure, IStyled<CurveStyle>
{
    [Reactive] public CurveStyle Style { get; set; }
    [Reactive] public ObjectState ObjectState { get; set; }

    public IEnumerable<Point> Attached => _attached.Select(x => x.Object);

    public IEnumerable<Point> ControlPoints => _points;

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

    private Vector2 CurveFunction(float t)
    {
        var tc = new float[4] 
        {
            (1f-t)*(1f-t)*(1f-t),
            3f*t*(1f-t)*(1f-t),
            3f*t*t*(1f-t),
            t*t*t
        };
        Vector2 p = new Vector2 { X=0f, Y=0f };
        for (int i = 0; i < 4; i++)
        {
            p.X += tc[i] * _points[i].X;
            p.Y += tc[i] * _points[i].Y;
        }
        return p;
    }

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
        foreach (var attachedPoint in _attached) 
        {
            var point = attachedPoint.Object;
            var t = attachedPoint.T;
            var newPos = CurveFunction(t);
            var dV = new Vector2(newPos.X - point.X, newPos.Y - point.Y);
        
            point.Move(dV);
        }
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

        // Stupid Linear Method (Should use logarithmical find)
        Vector2 minV = new Vector2();
        float minDst = float.PositiveInfinity;
        float minT = -1f;
        for(float t = 0f; t < 1f; t += 0.01f)
        {
            var point = CurveFunction(t);
            var distV = new Vector2(attachable.X - point.X, attachable.Y - point.Y);
            var dist = distV.Length();
            if (dist < minDst)
            {
                minV = distV;
                minDst = dist;
                minT = t;
            }
        }

        var attachedPoint = new AttachedPoint(attachable, minT);
        _attached.Add(attachedPoint);
        attachable.Move(minV);
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

        // Stupid Linear Method (Should use logarithmical find)
        for (float t = 0f; t < 1f; t += 0.01f)
        {
            var curPoint = CurveFunction(t);
            var distV = new Vector2(point.X - curPoint.X, point.Y - curPoint.Y);
            if (distV.Length() < distance)
            {
                return true;
            }
        }

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

        // Stupid Linear Method (Should use logarithmical find)
        Vector2 leftBottom = new Vector2(float.MaxValue, float.MaxValue); ;
        Vector2 rightTop = new Vector2(float.MaxValue, float.MaxValue); ;
        for (float t = 0f; t < 1f; t += 0.01f)
        {
            var point = CurveFunction(t);
            // TODO else if?
            if (point.X < leftBottom.X)
                leftBottom.X = point.X;
            if (point.X > rightTop.X)
                rightTop.X = point.X;
            if (point.Y < leftBottom.Y)
                leftBottom.Y = point.Y;
            if (point.Y > rightTop.Y)
                rightTop.Y = point.Y;
        }

        exportData.LeftBottomBound = leftBottom;
        exportData.RightTopBound = rightTop;

        return exportData;
    }

    public IGeometricObject Clone() => throw new NotImplementedException();
}
