using System.Numerics;
using Graphify.Geometry.Attachment;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.Export;
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
    public IEnumerable<Point> Attached { get => _attached.Select(x => x.Object); }

    /// <summary>
    /// Контрольные точки фигуры, по которым она строится
    /// </summary>
    public IEnumerable<Point> ControlPoints { get => [_pointA, _pointB]; }

    /// <summary>
    /// Стиль прямой
    /// </summary>
    [Reactive] public CurveStyle Style { get; set; }
    [Reactive] public ObjectState ObjectState { get; set; }

    /// <summary>
    /// Возвращает, может ли прямая менять своё положение за счёт методов перемещения фигуры
    /// </summary>
    internal bool CanBeMoved => !(_pointA.IsAttached || _pointB.IsAttached);


    private readonly List<AttachedPoint> _attached; //TODO: подумать над переходом на HashSet или любой другой *Set

    private readonly Point _pointA;
    private readonly Point _pointB;

    public Line(Point a, Point b, CurveStyle? style = null)
    {
        _pointA = a;
        _pointB = b;

        Style = style ?? CurveStyle.Default;
        _attached = [];
    }




    /// <summary>
    /// Добавляет присоединяемую точку <c>attachable</c> в своё множество присоединённых точек
    /// </summary>
    /// <param name="attachable"> - точка, которую необходимо присоединить к прямой</param>
    /// <exception cref="InvalidOperationException"> - если присоединить точку <c>attachable</c> к данной фигуре невозможно</exception>
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

        // Вычисление нового положения точки на прямой.
        // Точка будет располагаться на прямой, по кратчайшему расстоянию к этой прямой
        var A = new Vector2(_pointA.X, _pointA.Y); // Вспомогательные векторы для упрощения записи
        var B = new Vector2(_pointB.X, _pointB.Y);
        var T = new Vector2(attachable.X, attachable.Y);
        var ab = new Vector2(B.X - A.X, B.Y - A.Y);

        // Определяем растяжение вектора ab для позиционирования точки attachable
        var x = (T.Y - A.Y) * ab.Y + (-A.X + T.X) * ab.X;
        x /= ab.X * ab.X + ab.Y * ab.Y;

        var attachObj = new AttachedPoint(attachable);
        if (x < 0) // Если точка лежит левее точки a
        {
            var dv = new Vector2(A.X - T.X, A.Y - T.Y);
            attachable.Move(dv);
            attachObj.T = 0.0f;
        }
        else if (x > 1) // Если точка лежит левее точки b
        {
            var dv = new Vector2(B.X - T.X, B.Y - T.Y);
            attachable.Move(dv);
            attachObj.T = 1.0f;
        }
        else // Если точка лежит на прямой
        {
            var newPointCoords = new Vector2(A.X + x * ab.X, A.Y + x * ab.Y);
            var dv = new Vector2(newPointCoords.X - T.X, newPointCoords.Y - T.Y);
            attachable.Move(dv);
            attachObj.T = x;
        }

        _attached.Add(attachObj);
    }

    /// <summary>
    /// Удаляет присоединяемую точку <c>attachable</c> из своего множества присоединённых точек
    /// </summary>
    /// <param name="attachable"> - точка, которую необходимо отсоединить</param>
    /// <exception cref="InvalidOperationException"> - если точка <c>attachable</c> не является прикреплённой к фигуре</exception>
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

    /// <summary>
    /// Метод обновления положения всех зависимых от прямой точек
    /// </summary>
    public void Update()
    {
        var A = new Vector2(_pointA.X, _pointA.Y);
        var ab = new Vector2(_pointB.X - A.X, _pointB.Y - A.Y);

        foreach (var attachedPoint in _attached)
        {
            var t = attachedPoint.T;
            var newPointCoords = new Vector2(A.X + t * ab.X, A.Y + t * ab.Y);
            var dv = new Vector2(newPointCoords.X - attachedPoint.Object.X, newPointCoords.Y - attachedPoint.Object.Y);
            attachedPoint.Object.Move(dv);
        }
    }

    /// <summary>
    /// Отрисовка прямой на экране
    /// </summary>
    /// <param name="drawer"> - рисователь, реализующий интерфейс <c>IDrawer</c></param>
    public void Draw(IDrawer drawer)
    {
        Style.ApplyStyle(drawer);

        var begin = new Vector2(_pointA.X, _pointA.Y);
        var end = new Vector2(_pointB.X, _pointB.Y);

        drawer.DrawLine(begin, end, ObjectState);
    }

    /// <summary>
    /// Метод, определяющий, находится ли прямая на расстоянии <c>distance</c> от указанной точки <c>point</c>
    /// </summary>
    /// <param name="point"> - точка, относительно которой проверяется расстояние</param>
    /// <param name="distance"> - расстояние, в пределах которого выполняется проверка</param>
    /// <returns><c>true</c>, если точка <c>point</c> находится в пределах расстояния <c>distance</c> от прямой; <c>false</c> в ином случае</returns>
    /// <exception cref="ArgumentException"> - в случае, если <c>distance</c> не является строго положительным числом</exception>
    public bool IsNextTo(Vector2 point, float distance)
    {
        if (distance <= 0)
        {
            throw new ArgumentException($"Значение distance должно быть строго положительным числом. Ожидалось: distance > 0, получено: {distance}");
        }

        return distance > DistanceTo(point);
    }

    /// <summary>
    /// Метод, сдвигающий текущую прямую по направлению вектора <c>shift</c> на расстояние вектора <c>shift</c>.
    /// </summary>
    /// <param name="shift"> - вектор, относительно которого будет осуществляться сдвиг прямой</param>
    /// <exception cref="InvalidOperationException"> - если фигуру нельзя переместить (одна или несколько точек фигуры являются закреплёнными)</exception>
    public void Move(Vector2 shift)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var point in ControlPoints)
        {
            point.Move(shift);
        }
    }

    /// <summary>
    /// Метод, позволяющий вращать прямую относительно опорной точки <c>shift</c> на угол <c>angle</c> по часовой стрелке.
    /// </summary>
    /// <param name="shift"> - опорная точка, относительно которой осуществляется вращение прямой</param>
    /// <param name="angle"> - угол в градусах, на который поворачивается прямая по часовой стрелке</param>
    /// <exception cref="InvalidOperationException"> - если фигуру нельзя переместить (одна или несколько точек фигуры являются закреплёнными</exception>
    public void Rotate(Point shift, float angle)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var point in ControlPoints)
        {
            point.Rotate(shift, angle);
        }
    }

    /// <summary>
    /// Метод, позволяющий сделать зеркальное отражение с переворотом относительно заданной точки.
    /// </summary>
    /// <param name="point"> - точка, относительно которой происходит отражение</param>
    /// <exception cref="InvalidOperationException"> - если фигуру нельзя переместить (одна или несколько точек фигуры являются закреплёнными</exception>
    public void Reflect(Point point)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var objPoint in ControlPoints)
        {
            objPoint.Reflect(point);
        }
    }
    
    public FigureExportData GetExportData()
    {
        var leftBound = new Vector2(Math.Min(_pointA.X, _pointB.X), Math.Min(_pointA.Y, _pointB.Y));
        var rightBound = new Vector2(Math.Max(_pointA.X, _pointB.X), Math.Max(_pointA.Y, _pointB.Y));

        var exportData = new FigureExportData()
        {
            FigureType = ObjectType.Line,
            Style = Style,
            LeftBottomBound = leftBound,
            RightTopBound = rightBound,
        };
        return exportData;
    }

    internal float DistanceTo(Vector2 point)
    {
        // Точка будет располагаться на прямой, по кратчайшему расстоянию к этой прямой
        var A = new Vector2(_pointA.X, _pointA.Y); // Вспомогательные векторы для упрощения записи
        var B = new Vector2(_pointB.X, _pointB.Y);
        var T = point;
        var ab = new Vector2(B.X - A.X, B.Y - A.Y);
        var n = new Vector2(ab.Y, -ab.X);

        // Определяем растяжение вектора ab
        var x = (T.Y - A.Y) * ab.Y + (-A.X + T.X) * ab.X;
        x /= ab.X * ab.X + ab.Y * ab.Y;

        // Если точка point лежит левее точки a
        if (x < 0)
        {
            return (A - point).Length();
        }
        else if (x > 1) // Если точка point лежит правее точки b
        {
            return (B - point).Length();
        }

        // Если точка point лежит между точками a и b
        // Определяем растяжение нормали n
        var y = (A.X + x * ab.X - T.X) / ab.Y;
        var dist = y * n;  // Определяем расстояние от точки до прямой

        return dist.Length();
    }
    
    public IGeometricObject Clone()
    {
        var lineClone = new Line((Point)_pointA.Clone(), (Point)_pointB.Clone(),
            new CurveStyle(Style.PrimaryColor, Style.Name, Style.Size))
            {
                ObjectState = this.ObjectState
            };

        return lineClone;
    }
}
