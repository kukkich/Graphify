using System.Collections;
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


    private List<AttachedPoint> _attached; //TODO: подумать над переходом на HashSet или любой другой *Set

    private Point _pointA;
    private Point _pointB;

    public Line(Point A, Point B, CurveStyle? style = null)
    {
        _pointA = A;
        _pointB = B;

        Style = style ?? CurveStyle.Default;
        _attached = [];
    }

    


    /// <summary>
    /// Добавляет присоединяемую точку <c>attachable</c> в своё множество присоединённых точек
    /// </summary>
    /// <param name="attachable"> - точка, которую необходимо присоединить к прямой</param>
    public void ConsumeAttach(Point attachable)
    {
        if (ControlPoints.Contains(attachable))
        {
            return;
        }

        if (_attached.Find(x => x.Object == attachable) != null)
        {
            return;
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
        if (x < 0) // Если точка лежит левее точки A
        {
            var dv = new Vector2(A.X - T.X, A.Y - T.Y);
            attachable.Move(dv);
            attachObj.T = 0.0f;
        }
        else if (x > 1) // Если точка лежит левее точки B
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
    public void ConsumeDetach(Point attachable)
    {
        AttachedPoint? maybeAttached = _attached.Find(x => x.Object == attachable);
        if (maybeAttached != null)
        {
            _attached.Remove(maybeAttached);
        }
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

        drawer.DrawLine(begin, end);
    }

    /// <summary>
    /// Метод, определяющий, находится ли прямая на расстоянии <c>distance</c> от указанной точки <c>point</c>
    /// </summary>
    /// <param name="point"> - точка, относительно которой проверяется расстояние</param>
    /// <param name="distance"> - расстояние, в пределах которого выполняется проверка</param>
    /// <returns><c>true</c>, если точка <c>point</c> находится в пределах расстояния <c>distance</c> от прямой; <c>false</c> в ином случае</returns>
    public bool IsNextTo(Vector2 point, float distance)
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

        // Если точка point лежит левее точки A
        if (x < 0)
        {
            return _pointA.IsNextTo(point, distance);
        }
        else if (x > 1) // Если точка point лежит правее точки B
        {
            return _pointB.IsNextTo(point, distance);
        }

        // Если точка point лежит между точками A и B
        // Определяем растяжение нормали n
        var y = (A.X + x * ab.X - T.X) / ab.Y;
        var dist = y * n;  // Определяем расстояние от точки до прямой

        return distance > dist.Length();
    }

    /// <summary>
    /// Метод, сдвигающий текущую прямую по направлению вектора <c>shift</c> на расстояние вектора <c>shift</c>
    /// </summary>
    /// <param name="shift"> - вектор, относительно которого будет осуществляться сдвиг прямой</param>
    public void Move(Vector2 shift)
    {
        // TODO: запретить действие, если одна или несколько точек являются прикреплёнными
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
    public void Rotate(Point shift, float angle)
    {
        // TODO: запретить действие, если одна или несколько точек являются прикреплёнными
        foreach (var point in ControlPoints)
        {
            point.Rotate(shift, angle);
        }
    }

    /// <summary>
    /// Метод, позволяющий сделать зеркальное отражение с переворотом относительно заданной точки.
    /// </summary>
    /// <param name="point"> - точка, относительно которой происходит отражение</param>
    public void Reflect(Point point)
    {
        // TODO: запретить действие, если одна или несколько точек являются прикреплёнными
        foreach (var objPoint in ControlPoints)
        {
            objPoint.Reflect(point);
        }
    }

    public FigureExportData GetExportData() => throw new NotImplementedException();

    //переопределили метод для сравнения объектов
    public override bool Equals(object obj)
    {
        if (obj.GetType() != this.GetType()) return false;

        var other = (Line)obj;
        return (Point.Equals(this._pointA, other._pointA) && Point.Equals(this._pointB, other._pointB));
    }
}
