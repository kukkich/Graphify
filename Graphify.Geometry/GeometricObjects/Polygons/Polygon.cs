using System.Numerics;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class Polygon : ReactiveObject, IFigure, IStyled<PolygonStyle>
{
    public IEnumerable<Point> Attached
    {
        get
        {
            var attachedPoints = new List<Point>();
            foreach (var line in _lines)
            {
                attachedPoints.AddRange(line.Attached);
            }

            return attachedPoints;
        }
    }
    public IEnumerable<Point> ControlPoints
    {
        get
        {
            var controlPoints = new HashSet<Point>();
            foreach (var point in _lines.SelectMany(line => line.ControlPoints))
            {
                controlPoints.Add(point);
            }

            return controlPoints;
        }
    }
    [Reactive] public PolygonStyle Style { get; set; }
    [Reactive] public ObjectState ObjectState { get; set; }

    internal bool CanBeMoved
    {
        get
        {
            return _lines.All(line => line.CanBeMoved);
        }
    }

    private readonly List<Line> _lines;


    /// <summary>
    /// Конструктор класса Polygon
    /// </summary>
    /// <param name="points"> - массив опорных точек полигона (должен иметь структуру вида [A, B, C, ..., A] и минимальную длину 3)</param>
    /// <param name="style"> - стиль полигона. <c>PolygonStyle.Default</c>, если <c>null</c></param>
    /// <exception cref="InvalidDataException"> - исключение в случае, если размер <c>points</c> < 3</exception>
    public Polygon(Point[] points, PolygonStyle? style = null)
    {
        if (points.Length < 3)
        {
            throw new InvalidDataException($"Невозможно создать полигон. Необходимо 3+ точек, а передано {points.Length}");
        }

        _lines = [];
        for (int i = 0; i < points.Length - 1; i++)
        {
            _lines.Add(new Line(points[i], points[i + 1]));
        }
        if (points[0] != points[^1])
        {
            _lines.Add(new Line(points[^1], points[0]));
        }

        Style = style ?? PolygonStyle.Default;
    }


    public void Update()
    {
        foreach (var line in _lines)
        {
            line.Update();
        }
    }

    /// <summary>
    /// Метод, определяющий, находится ли какая-либо из граней полигона на расстоянии <c>distance</c> от указанной точки <c>point</c>
    /// </summary>
    /// <param name="point"> - точка, относительно которой проверяется расстояние</param>
    /// <param name="distance"> - расстояние, в пределах которого выполняется проверка</param>
    /// <returns><c>true</c>, если точка <c>point</c> находится в пределах расстояния <c>distance</c> от грани полигона; <c>false</c> в ином случае</returns>
    /// <exception cref="ArgumentException"> - в случае, если <c>distance</c> не является строго положительным числом</exception>
    public bool IsNextTo(Vector2 point, float distance)
    {
        if (distance <= 0)
        {
            throw new ArgumentException($"Значение distance должно быть строго положительным числом. Ожидалось: distance > 0, получено: {distance}");
        }

        var isNextToLines = _lines.Where(line => line.IsNextTo(point, distance));

        return isNextToLines.Any();
    }

    bool IGeometricObject.CanBeMoved() => CanBeMoved;

    /// <summary>
    /// Метод, сдвигающий текущий полигон по направлению вектора <c>shift</c> на расстояние вектора <c>shift</c>.
    /// </summary>
    /// <param name="shift"> - вектор, относительно которого будет осуществляться сдвиг полигона</param>
    /// <exception cref="InvalidOperationException"> - если фигуру нельзя переместить (одна или несколько точек фигуры являются закреплёнными)</exception>
    public void Move(Vector2 shift)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var p in ControlPoints)
        {
            p.Move(shift);
        }
    }

    /// <summary>
    /// Добавляет присоединяемую точку <c>attachable</c> к ближайшей грани полигона
    /// </summary>
    /// <param name="attachable"> - точка, которую необходимо присоединить к полигону</param>
    /// <exception cref="InvalidOperationException"> - если присоединить точку <c>attachable</c> к данной фигуре невозможно</exception>
    public void ConsumeAttach(Point attachable)
    {
        if (ControlPoints.Contains(attachable))
        {
            throw new InvalidOperationException("Нельзя присоединить точку к данной фигуре: точка является опорной для данной фигуры");
        }
        if (Attached.Contains(attachable))
        {
            throw new InvalidOperationException("Нельзя присоединить точку к данной фигуре: точка уже присоединена к данной фигуре");
        }

        var nearestEdge = _lines.MinBy(e => e.DistanceTo(new Vector2(attachable.X, attachable.Y))) ?? throw new InvalidOperationException("Произошла ошибка в Polygon.ConsumeAttach: _lines.MinBy() вернула null. Интересный косяк");
        nearestEdge.ConsumeAttach(attachable);
    }

    /// <summary>
    /// Удаляет присоединяемую точку <c>attachable</c> из своего множества присоединённых точек
    /// </summary>
    /// <param name="attachable"> - точка, которую необходимо отсоединить</param>
    /// <exception cref="InvalidOperationException"> - если точка <c>attachable</c> не является прикреплённой к фигуре</exception>
    public void ConsumeDetach(Point attachable)
    {
        var maybeFindedEdge = _lines.Find(e => e.Attached.Contains(attachable));
        if (maybeFindedEdge is not null)
        {
            maybeFindedEdge.ConsumeDetach(attachable);
            return;
        }

        throw new InvalidOperationException("Нельзя отсоединить точку от данной фигуры: эта точка не является прикреплённой к данной фигуре");
    }

    /// <summary>
    /// Метод, позволяющий вращать полигон относительно опорной точки <c>shift</c> на угол <c>angle</c> по часовой стрелке.
    /// </summary>
    /// <param name="shift"> - опорная точка, относительно которой осуществляется вращение полигона</param>
    /// <param name="angle"> - угол в градусах, на который поворачивается прямая по часовой стрелке</param>
    /// <exception cref="InvalidOperationException"> - если фигуру нельзя переместить (одна или несколько точек фигуры являются закреплёнными</exception>
    public void Rotate(Point shift, float angle)
    {
        if (!CanBeMoved)
        {
            throw new InvalidOperationException("Невозможно выполнить перемещение фигуры: одна или несколько точек фигуры являются закреплёнными");
        }

        foreach (var p in ControlPoints)
        {
            p.Rotate(shift, angle);
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

        foreach (var p in ControlPoints)
        {
            p.Reflect(point);
        }
    }

    /// <summary>
    /// Метод, отрисовывающий на экране полигон и отдельно каждую его грань
    /// </summary>
    /// <param name="drawer"> - рисователь, реализующий интерфейс <c>IDrawer</c></param>
    public void Draw(IDrawer drawer)
    {
        Style.ApplyStyle(drawer);

        var points = ControlPoints.Select(point => new Vector2(point.X, point.Y))
            .ToList();

        drawer.DrawPolygon(points, ObjectState);
        foreach (var line in _lines)
        {
            line.Draw(drawer);
        }
    }

    public FigureExportData GetExportData()
    {
        var points = ControlPoints;
        var minX = points.Min(e => e.X);
        var minY = points.Min(e => e.Y);
        var maxX = points.Max(e => e.X);
        var maxY = points.Max(e => e.Y);

        var leftBound = new Vector2(minX, minY);
        var rightBound = new Vector2(maxX, maxY);

        var exportData = new FigureExportData()
        {
            FigureType = ObjectType.Polygon,
            Style = Style,
            LeftBottomBound = leftBound,
            RightTopBound = rightBound
        };
        return exportData;
    }

    public IGeometricObject Clone()
    {
        var pointsClones = ControlPoints.Select(c => (Point)c.Clone()).ToArray();

        var polygonClone =
            new Polygon(pointsClones, new PolygonStyle(Style.PrimaryColor, Style.LineColor, Style.Name, Style.Size))
            {
                ObjectState = ObjectState
            };

        return polygonClone;
    }
}
