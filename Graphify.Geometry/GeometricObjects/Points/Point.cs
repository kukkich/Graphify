using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Points;

public class Point : ReactiveObject, IGeometricObject, IAttachable, IStyled<PointStyle>
{
    /// <summary>
    /// Координаты точки по оси X.
    /// Изменения координат точки происходят встроенными методами
    /// </summary>
    [Reactive] public float X { get; private set; }

    /// <summary>
    /// Координаты точки по оси Y.
    /// Изменения координат точки происходят встроенными методами
    /// </summary>
    [Reactive] public float Y { get; private set; }

    /// <summary>
    /// Фигура, к которой присоединена точка.
    /// Имеет значение NULL, если точка не присоединена
    /// </summary>
    public IFigure? AttachedTo { get; private set; }

    /// <summary>
    /// Возвращает <c>true</c>, если точка присоединена к фигуре
    /// </summary>
    public bool IsAttached => AttachedTo != null;

    /// <summary>
    /// Список фигур, которые задаются (управляются) данной точкой.
    /// Может быть пустым
    /// </summary>
    public IEnumerable<IFigure> ControlFor => _controlFor;

    /// <summary>
    /// Стиль точки
    /// </summary>
    [Reactive] public PointStyle Style { get; set; }

    private List<IFigure> _controlFor;

    public Point(float x, float y, PointStyle? style = null)
    {
        X = x;
        Y = y;
        Style = style ?? PointStyle.Default;

        _controlFor = new List<IFigure>(); 
    }

    /// <summary>
    /// Метод, проверяющий, находится ли текущая точка возле точки <c>point</c> в радиусе <c>distance</c>
    /// </summary>
    /// <param name="point"> - точка, относительно которой выполняется проверка</param>
    /// <param name="distance"> - расстояние, в радиусе которого выполняется проверка</param>
    /// <returns>логическое значение: находится ли точка <c>point</c> в радиусе <c>distance</c> от текущей точки</returns>
    public bool IsNextTo(Vector2 point, float distance)
    {
        var dx = X - point.X;
        var dy = Y - point.Y;

        var dist = Math.Sqrt(dy * dy + dx * dx);
        return dist < distance;
    }

    /// <summary>
    /// Метод, перемещающий точку в пространстве по вектору <c>shift</c>.
    /// Вместе со своим перемещением, обновляет все фигуры, которые к точке привязаны, вызовом метода <c>Point.Update()</c>
    /// </summary>
    /// <param name="shift"> - вектор сдвига точки в пространстве</param>
    public void Move(Vector2 shift) 
    {
        if (IsAttached)
        {
            // Колхоз time - сначала открепляем точку, затем её двигаем, а потом прикрепляем обратно
            var attachedObj = AttachedTo;
            Detach();

            X += shift.X;
            Y += shift.Y;

            AttachTo(attachedObj);
        }
        else
        {
            X += shift.X;
            Y += shift.Y;
        }

        Update();
    }

    /// <summary>
    /// Метод, вращающий точку относительно точки <c>shift</c> на угол <c>angle</c> по часовой стрелке.
    /// В случае, если точка закреплена на фигуре, этот метод не сделает ничего
    /// </summary>
    /// <param name="shift"> - точка, относительно которой будет совершаться вращение текущей точки</param>
    /// <param name="angle"> - угол вращения точки в градусах</param>
    public void Rotate(Point shift, float angle)
    {
        if (IsAttached)
        {
            return;
        }

        var radians = angle * Math.PI / 180.0;
        var s = (float)Math.Sin(radians);
        var c = (float)Math.Cos(radians);

        (X, Y) = (c * X - s * Y, s * X + c * Y);
        Update();
    }

    /// <summary>
    /// Метод, симметрично отражающий текущую точку относительно точки <c>point</c>.
    /// Если точка является закреплённой к фигуре, то метод не сделает ничего.
    /// </summary>
    /// <param name="point" - точка, относительно которой будет происходить отражение></param>
    public void Reflect(Point point)
    {
        if (IsAttached)
        {
            return;
        }

        var dx = point.X - X;
        var dy = point.Y - Y;

        X += 2 * dx;
        Y += 2 * dy;

        Update();
    }

    /// <summary>
    /// Обновляет фигуры, привязанные к данной точке
    /// </summary>
    public void Update()
    {
        foreach (var fig in ControlFor)
        {
            fig.Update();
        }
    }

    /// <summary>
    /// Проверяет, может ли точка быть присоединённой к фигуре <c>consumer</c>
    /// </summary>
    /// <param name="consumer"> - фигура, к которой необходимо выполнить проверку присоединения точки</param>
    /// <returns> true, если присоединить точку возможно</returns>
    public bool CanAttachTo(IFigure consumer)
    {
        if (AttachedTo != null)
        {
            return false;
        }

        // Проверка на то, что фигура, к которой привязывается наша точка, косвенно зависит от нашей точки
        var figures = new HashSet<IFigure>();           // Для отслеживания ещё не посещённых фигур
        var visitedFigures = new HashSet<IFigure>();    // Для отслеживания уже или в скором времени посещённых фигур
        _ = figures.Add(consumer);
        _ = visitedFigures.Add(consumer);

        while (figures.Count != 0)
        {
            var figure = figures.First();

            var points = figure.ControlPoints;
            foreach(var point in points)
            {
                if (point == this)
                {
                    return false;
                }

                foreach(var fig in point.ControlFor)
                {
                    bool isAdded = visitedFigures.Add(fig);
                    if (isAdded)
                    {
                        figures.Add(fig);
                    }
                }
            }

            figures.Remove(figure);
        }

        // На удалённость проверку не делаем. В случае чего просто перемещаем точку к месту прикрепления
        return true;
    }

    /// <summary>
    /// Метод, побуждающий точку присоединиться к фигуре <c>consumer</c>.
    /// В случае невозможности присоединения не делает ничего.
    /// </summary>
    /// <param name="consumer"> - фигура, к которой необходимо присоединить текущую точку</param>
    public void AttachTo(IFigure consumer)
    {
        if (!CanAttachTo(consumer))
        {
            return;
        }
        AttachedTo = consumer;
        consumer.ConsumeAttach(this);

        Update();
    }

    /// <summary>
    /// Метод, побуждающий точку отсоединиться от фигуры.
    /// Если точка ни к кому не присоединена, то ничего не произойдёт.
    /// </summary>
    public void Detach()
    {
        if (AttachedTo == null)
        {
            return;
        }
        AttachedTo.ConsumeDetach(this);
        AttachedTo = null;

        Update();
    }

    /// <summary>
    /// Метод, отрисовывающий точку, используя графические примитивы
    /// </summary>
    /// <param name="drawer"> - рисователь, предоставляющий набор примитивов для отрисовки</param>
    public void Draw(IDrawer drawer)
    {
        Style.ApplyStyle(drawer);

        var p = new Vector2(X, Y);

        drawer.DrawPoint(p);
    }

}
