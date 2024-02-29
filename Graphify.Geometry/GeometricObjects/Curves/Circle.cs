using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Attachment;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Curves;

public class Circle : ReactiveObject, IFigure, IStyled<CurveStyle>
{
    /// <summary>
    /// Список точек, прикреплённых к данному объекту
    /// </summary>
    public IEnumerable<Point> Attached => _attached.Select(x => x.Object);

    /// <summary>
    /// Контрольные точки фигуры, по которым она строится
    /// </summary>
    public IEnumerable<Point> ControlPoints => [_centerPoint, _radiusPoint];

    /// <summary>
    /// Стиль окружности
    /// </summary>
    [Reactive] public CurveStyle Style { get; set; }

    /// <summary>
    /// Возвращает, может ли прямая менять своё положение за счёт методов перемещения фигуры
    /// </summary>
    private bool CanBeMoved => !(_centerPoint.IsAttached || _radiusPoint.IsAttached);

    /// <summary>
    /// Вычисляет и возвращает радиус окружности
    /// </summary>
    private float Radius => (float)Math.Sqrt(Math.Pow(_centerPoint.X - _radiusPoint.X, 2) + Math.Pow(_centerPoint.Y - _radiusPoint.Y, 2));


    private List<AttachedPoint> _attached; //TODO: подумать над переходом на HashSet или любой другой *Set

    private Point _centerPoint;
    private Point _radiusPoint;


    public Circle(Point center, Point radius, CurveStyle? style = null)
    {
        _centerPoint = center;
        _radiusPoint = radius;

        Style = style ?? CurveStyle.Default;
        _attached = [];
    }

    public void Update()
    {
        var radius = Radius;

        foreach(var attachedPoint in _attached)
        {
            var point = attachedPoint.Object;
            var attachedParam = attachedPoint.T;
            var angle = attachedParam * (2 * Math.PI); // [0; 1) --> [0; 2pi)
            var newPosition = new Vector2((float)(radius * Math.Cos(angle)), (float)(radius * Math.Sin(angle)));
            var dv = new Vector2(newPosition.X - point.X, newPosition.Y - point.Y);

            point.Move(dv);
        }
    }
    
    /// <summary>
    /// Добавляет присоединяемую точку <c>attachable</c> в своё множество присоединённых точек
    /// </summary>
    /// <param name="attachable"> - точка, которую необходимо присоединить к прямой</param>
    public void ConsumeAttach(Point attachable)
    {
        if (ControlPoints.Contains(attachable))
        {
            return; // TODO: заменить на exception
        }

        if (_attached.Find(x => x.Object == attachable) != null)
        {
            return; // TODO: заменить на exception
        }

        var centerToPointVec = new Vector2(attachable.X - _centerPoint.X, attachable.Y - _centerPoint.Y);
        var length = Math.Sqrt(centerToPointVec.X * centerToPointVec.X + centerToPointVec.Y * centerToPointVec.Y);
        var angle = Math.Acos(centerToPointVec.X / length) * double.Sign(Math.Asin(centerToPointVec.Y / length));
        angle = (angle + 2 * Math.PI) % (2*Math.PI);  // Округляем угол до нужного диапазона [0; 2pi)

        var radius = Radius;
        var newPosition = new Vector2((float)(radius * Math.Cos(angle)), (float)(radius * Math.Sin(angle)));
        var moveVector = new Vector2(newPosition.X - attachable.X, newPosition.Y - attachable.Y);

        var angleParameter = angle / (2 * Math.PI); // [0; 2pi) --> [0; 1)

        var attachedPoint = new AttachedPoint(attachable, (float)angleParameter);
        _attached.Add(attachedPoint);
        attachable.Move(moveVector);
    }

    /// <summary>
    /// Удаляет присоединённую точку <c>attachable</c> из своего множества присоединённых точек
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
    /// Метод, проверяющий, находится ли указанная точка <c>point</c> на расстоянии <c>distance</c> от текущей окружности.
    /// Расстояние замеряется по окружности, а не по кругу.
    /// </summary>
    /// <param name="point"> - точка, относительно которой выполняется проверка расстояния</param>
    /// <param name="distance"> - расстояние, в пределах которого выполняется проверка</param>
    /// <returns><c>true</c>, если точка <c>point</c> находится в пределах расстояния <c>distance</c> от прямой; <c>false</c> в ином случае</returns>
    public bool IsNextTo(Vector2 point, float distance)
    {
        var dvToPoint = new Vector2(_centerPoint.X - point.X, _centerPoint.Y - point.Y);
        var distanceToPoint = (float)Math.Sqrt(dvToPoint.X * dvToPoint.X + dvToPoint.Y * dvToPoint.Y);

        return Math.Abs(Radius - distanceToPoint) < distance;
    }

    /// <summary>
    /// Метод, сдвигающий текущую окружность по направлению вектора <c>shift</c> на расстояние вектора <c>shift</c>.
    /// Если окружность имеет хотя бы одну закреплённую точку, то данный метод не делает ничего.
    /// </summary>
    /// <param name="shift"> - вектор, относительно которого будет осуществляться сдвиг окружности</param>
    public void Move(Vector2 shift)
    {
        if (!CanBeMoved)
        {
            return; //TODO: бросить исключение о невозможности перемещения фигуры
        }

        foreach (var objPoint in ControlPoints)
        {
            objPoint.Move(shift);
        }
    }

    /// <summary>
    /// Метод, позволяющий вращать окружность относительно опорной точки <c>shift</c> на угол <c>angle</c> по часовой стрелке.
    /// Если окружность имеет хотя бы одну закреплённую точку, то данный метод не делает ничего.
    /// </summary>
    /// <param name="shift"> - опорная точка, относительно которой осуществляется вращение окружности</param>
    /// <param name="angle"> - угол в градусах, на который поворачивается прямая по часовой стрелке</param>
    public void Rotate(Point shift, float angle)
    {
        if (!CanBeMoved)
        {
            return; //TODO: бросить исключение о невозможности перемещения фигуры
        }

        foreach (var objPoint in ControlPoints)
        {
            objPoint.Rotate(shift, angle);
        }
    }
    
    /// <summary>
    /// Метод, позволяющий сделать зеркальное отражение с переворотом относительно заданной точки.
    /// Если окружность имеет хотя бы одну закреплённую точку, то данный метод не делает ничего.
    /// </summary>
    /// <param name="point"> - точка, относительно которой происходит отражение</param>
    public void Reflect(Point point)
    {
        if (!CanBeMoved)
        {
            return; //TODO: бросить исключение о невозможности перемещения фигуры
        }

        foreach (var objPoint in ControlPoints)
        {
            objPoint.Reflect(point);
        }
    }
    
    /// <summary>
    /// Отрисовка окружности на экране
    /// </summary>
    /// <param name="drawer"> - рисователь, реализующий интерфейс <c>IDrawer</c></param>
    public void Draw(IDrawer drawer)
    {
        Style.ApplyStyle(drawer);
        var centerPoint = new Vector2(_centerPoint.X, _centerPoint.Y);

        drawer.DrawCircle(centerPoint, Radius);
    }
}
