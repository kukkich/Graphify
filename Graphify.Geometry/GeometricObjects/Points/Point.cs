using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Curves;
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
    [Reactive] public ObjectState ObjectState { get; set; }

    private readonly List<IFigure> _controlFor;

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
    /// <exception cref="ArgumentException"> - в случае, если <c>distance</c> не является строго положительным числом</exception>
    public bool IsNextTo(Vector2 point, float distance)
    {
        if (distance <= 0)
        {
            throw new ArgumentException($"Значение distance должно быть строго положительным числом. Ожидалось: distance > 0, получено: {distance}");
        }

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
    /// </summary>
    /// <param name="shift"> - точка, относительно которой будет совершаться вращение текущей точки</param>
    /// <param name="angle"> - угол вращения точки в градусах</param>
    /// <exception cref="InvalidOperationException"> - если точка является закреплённой</exception>
    public void Rotate(Point shift, float angle)
    {
        if (IsAttached)
        {
            throw new InvalidOperationException("Невозможно повернуть точку: точка является закреплённой");
        }

        var x = X - shift.X;
        var y = Y - shift.Y;

        var radians = -angle * Math.PI / 180.0;
        var s = (float)Math.Sin(radians);
        var c = (float)Math.Cos(radians);

        (x, y) = (c * x - s * y, s * x + c * y);
        (X, Y) = (x + shift.X, y + shift.Y);

        Update();
    }

    /// <summary>
    /// Метод, симметрично отражающий текущую точку относительно точки <c>point</c>.
    /// </summary>
    /// <param name="point" - точка, относительно которой будет происходить отражение></param>
    /// <exception cref="InvalidOperationException"> - если точка является закреплённой</exception>
    public void Reflect(Point point)
    {
        if (IsAttached)
        {
            throw new InvalidOperationException("Невозможно отразить точку: точка является закреплённой");
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
    /// <returns> <c>true</c>, если присоединить точку возможно; <c>else</c> в ином случае</returns>
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
            foreach (var point in points)
            {
                if (point == this)
                {
                    return false;
                }

                foreach (var fig in point.ControlFor)
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
    /// </summary>
    /// <param name="consumer"> - фигура, к которой необходимо присоединить текущую точку</param>
    /// <exception cref="InvalidOperationException"> - если закрепление точки невозможно</exception>
    public void AttachTo(IFigure consumer)
    {
        if (!CanAttachTo(consumer))
        {
            throw new InvalidOperationException("Закрепление точки невозможно: точка является закреплённой либо имеет косвенную зависимость от прикрепляемой фигуры");
        }
        consumer.ConsumeAttach(this);
        AttachedTo = consumer;

        Update();
    }

    /// <summary>
    /// Метод, побуждающий точку отсоединиться от фигуры.
    /// </summary>
    /// <exception cref="InvalidOperationException"> - если открепить точку невозможно</exception>
    public void Detach()
    {
        if (AttachedTo == null)
        {
            throw new InvalidOperationException("Открепление точки невозможно: точка ни к кому не закреплена");
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

        drawer.DrawPoint(p, ObjectState);
    }


    public PointExportData GetExportData()
    {
        return new PointExportData(new Vector2(X, Y), Style);
    }

    public IGeometricObject Clone()
    {
        var pointClone = new Point(X, Y,
            new PointStyle(new CurveStyle(Style.PrimaryColor, Style.Name, Style.Size), Style.Variant))
        {
            ObjectState = ObjectState
        };

        return pointClone;
    }
}
