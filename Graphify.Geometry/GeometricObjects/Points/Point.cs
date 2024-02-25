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
    public IFigure? AttachedTo { get; private set; } // TODO: как обрабатывать присоединение к одной грани полигона??

    public bool IsAttached { get; } //TODO: реализовать

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
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public bool IsNextTo(Vector2 point, float distance) => throw new NotImplementedException();

    /// <summary>
    /// Метод, перемещающий точку в пространстве по вектору <c>shift</c>.
    /// Вместе со своим перемещением, обновляет все фигуры, которые к точке привязаны, вызовом метода <c>Point.Update()</c>
    /// </summary>
    /// <param name="shift"> - вектор сдвига точки в пространстве</param>
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public void Move(Vector2 shift) 
    {
        X += shift.X;
        Y += shift.Y;

        //TODO: Стоит ли вызывать здесь метод Point.Update()? В случае перемещения полигона, полигон будет перерисован N раз вместо 1
    }

    /// <summary>
    /// Метод, вращающий точку относительно точки <c>shift</c> на угол <c>angle</c> по часовой стрелке
    /// </summary>
    /// <param name="shift"> - точка, относительно которой будет совершаться вращение текущей точки</param>
    /// <param name="angle"> - угол вращения точки в градусах</param>
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public void Rotate(Point shift, float angle)
    {
        var radians = angle * Math.PI / 180.0;
        var s = (float)Math.Sin(radians);
        var c = (float)Math.Cos(radians);

        (X, Y) = (c * X - s * Y, s * X + c * Y);
        //TODO: Стоит ли вызывать здесь метод Point.Update()? В случае перемещения полигона, полигон будет перерисован N раз вместо 1
    }

    /// <summary>
    /// Метод, симметрично отражающий текущую точку относительно точки <c>point</c>
    /// </summary>
    /// <param name="point" - точка, относительно которой будет происходить отражение></param>
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public void Reflect(Point point) => throw new NotImplementedException();

    /// <summary>
    /// Обновляет фигуры, привязанные к данной точке
    /// </summary>
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public void Update() => throw new NotImplementedException();

    /// <summary>
    /// Проверяет, может ли точка быть присоединённой к фигуре <c>consumer</c>
    /// </summary>
    /// <param name="consumer"> - фигура, к которой необходимо выполнить проверку присоединения точки</param>
    /// <returns> true, если присоединить точку возможно</returns>
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public bool CanAttachTo(IFigure consumer) => throw new NotImplementedException();

    /// <summary>
    /// Метод, побуждающий точку присоединиться к фигуре <c>consumer</c>.
    /// В случае невозможности присоединения не делает ничего.
    /// </summary>
    /// <param name="consumer"> - фигура, к которой необходимо присоединить текущую точку</param>
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public void AttachTo(IFigure consumer) => throw new NotImplementedException();
    public void Detach() => throw new NotImplementedException();

    //TODO: сделать метод присоединения фигуры к данной точке

    /// <summary>
    /// Метод, отрисовывающий точку, используя графические примитивы
    /// </summary>
    /// <param name="drawer"> - рисователь, предоставляющий набор примитивов для отрисовки</param>
    /// <exception cref="NotImplementedException"> - исключение, ибо метод не реализован</exception>
    public void Draw(IDrawer drawer) => throw new NotImplementedException();

}
