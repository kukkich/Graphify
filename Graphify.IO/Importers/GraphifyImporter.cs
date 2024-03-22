using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Polygons;
using Graphify.IO.Interfaces;
using Graphify.IO.JSON.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Graphify.IO.Importers;

public partial class GraphifyImporter : IImporter
{
    private const byte ShiftToOpeningBracket = 8;

    private readonly ILogger<GraphifyImporter> _logger;

    private readonly Dictionary<uint, Point> _points = [];

    private readonly List<IFigure> _figures = [];

    private List<JsonPointObject>? _jsonPointObjects = [];

    private List<JsonFigureObject>? _jsonFigureObjects = [];

    public GraphifyImporter(ILogger<GraphifyImporter> logger)
    {
        _logger = logger;
    }

    public ImportResult ImportFrom(string path)
    {
        Clear();

        string jsonString = File.ReadAllText(path);

        int indexItem1 = jsonString.IndexOf("Item1") + ShiftToOpeningBracket;
        int indexItem2 = jsonString.IndexOf("Item2") + ShiftToOpeningBracket;
        int lengthItem1 = indexItem2 - indexItem1 - 14;

        string jsonStringItem1 = jsonString.Substring(indexItem1, lengthItem1);
        string jsonStringItem2 = jsonString[indexItem2..jsonString.LastIndexOf('}')];

        _jsonPointObjects = JsonConvert.DeserializeObject<List<JsonPointObject>>(jsonStringItem1);
        _jsonFigureObjects = JsonConvert.DeserializeObject<List<JsonFigureObject>>(jsonStringItem2);

        if (_jsonFigureObjects is null && _jsonPointObjects is null)
        {
            _logger.LogError("There is no data to import!");
            throw new InvalidDataException("There is no data to import!");
        }

        if (_jsonFigureObjects is not null && _jsonPointObjects is null)
        {
            _logger.LogError("There can be no shapes without dots!");
            throw new InvalidDataException("There can be no shapes without dots!");
        }

        if (_jsonFigureObjects is null && _jsonPointObjects is not null)
        {
            AddPoints();

            _logger.LogDebug("Successfully import!");

            return new ImportResult() { Figures = _figures, Points = _points.Values };
        }

        AddPoints();
        AddFigures();

        _logger.LogDebug("Successfully import!");

        return new ImportResult() { Points = _points.Values, Figures = _figures };
    }

    private void AddFigures()
    {
        foreach (JsonFigureObject figureObject in _jsonFigureObjects!)
        {
            switch (figureObject.ObjectType)
            {
                case ObjectType.Circle: AddCircle(figureObject); break;
                case ObjectType.Line: AddLine(figureObject); break;
                case ObjectType.CubicBezier: AddCubicBezire(figureObject); break;
                case ObjectType.Polygon: AddPolygon(figureObject); break;
            }
        }
    }

    private Point FindPoint(uint id)
    {
        if (_points.TryGetValue(id, out var point))
        {
            return point;
        }

        throw new InvalidDataException($"Invalid id point: {id}.The figure contains a non-existent point!");
    }

    private List<Point>? CreateListPoints(uint[]? idPoints)
    {
        if (idPoints == null)
        {
            return null;
        }

        List<Point> points = [];

        foreach (uint idPoint in idPoints)
        {
            Point point = FindPoint(idPoint);

            points.Add(point);
        }

        return points;
    }

    private void AddPoints()
    {
        foreach (JsonPointObject pointObject in _jsonPointObjects!)
        {
            _points.Add(pointObject.Id, new Point(pointObject.Position.X, pointObject.Position.Y, pointObject.Style));
        }
    }

    private void AddLine(JsonFigureObject lineObject)
    {
        CurveStyle lineStyle = new CurveStyle(
                            lineObject.Style.PrimaryColor,
                            lineObject.Style.Name,
                            (lineObject.Style as CurveStyle ?? CurveStyle.Default).Size
                        );

        List<Point>? controlPoints = CreateListPoints(lineObject.ControlPoints);
        List<Point>? attachedPoints = CreateListPoints(lineObject.AttachedPoint);

        if (controlPoints is null || (controlPoints.Count > 2))
        {
            _logger.LogError("The number of points to build a line is not equal to 2. The number of points contained: {pointsCount}", controlPoints?.Count);
            throw new ArgumentException("");
        }

        Line line = new(controlPoints[0], controlPoints[1], lineStyle);

        if (attachedPoints is not null)
        {
            foreach (Point point in attachedPoints)
            {
                point.AttachTo(line);
            }
        }

        _figures.Add(line);
    }

    private void AddCircle(JsonFigureObject circleObject)
    {
        CurveStyle circleStyle = new CurveStyle(
                    circleObject.Style.PrimaryColor,
                    circleObject.Style.Name,
                    (circleObject.Style as CurveStyle ?? CurveStyle.Default).Size
                );

        List<Point>? controlPoints = CreateListPoints(circleObject.ControlPoints);
        List<Point>? attachedPoints = CreateListPoints(circleObject.AttachedPoint);

        if (controlPoints is null || (controlPoints.Count > 2))
        {
            _logger.LogError("The number of points to build a circle is not equal to 2. The number of points contained: {pointsCount}", controlPoints?.Count);
            throw new ArgumentException("");
        }

        Circle circle = new(controlPoints[0], controlPoints[1], circleStyle);

        if (attachedPoints is not null)
        {
            foreach (Point point in attachedPoints)
            {
                point.AttachTo(circle);
            }
        }

        _figures.Add(circle);
    }

    private void AddPolygon(JsonFigureObject polygonObject)
    {
        PolygonStyle polygonStyle = new PolygonStyle(
            polygonObject.Style.PrimaryColor,
            polygonObject.Style.Name
        );

        List<Point>? controlPoints = CreateListPoints(polygonObject.ControlPoints);
        List<Point>? attachedPoints = CreateListPoints(polygonObject.AttachedPoint);

        if (controlPoints is null || (controlPoints.Count < 3))
        {
            _logger.LogError("The number of points to build a polygon is less than 3. The number of points contained: {pointsCount}", controlPoints?.Count);
            throw new ArgumentException("");
        }

        Point[] points = [.. controlPoints];
        Polygon polygon = new(points, polygonStyle);

        if (attachedPoints is not null)
        {
            foreach (Point point in attachedPoints)
            {
                point.AttachTo(polygon);
            }
        }

        _figures.Add(polygon);
    }

    private void AddCubicBezire(JsonFigureObject cubicBezireObject)
    {
        CurveStyle cubicBezireStyle = new CurveStyle(
                    cubicBezireObject.Style.PrimaryColor,
                    cubicBezireObject.Style.Name,
                    (cubicBezireObject.Style as CurveStyle ?? CurveStyle.Default).Size
                );

        List<Point>? controlPoints = CreateListPoints(cubicBezireObject.ControlPoints);
        List<Point>? attachedPoints = CreateListPoints(cubicBezireObject.AttachedPoint);

        if (controlPoints is null || (controlPoints.Count != 4))
        {
            _logger.LogError("The number of points to build a cubic bezire is not equal to 4. The number of points contained: {pointsCount}", controlPoints?.Count);
            throw new ArgumentException("");
        }

        Point[] points = [.. controlPoints];
        CubicBezierCurve cubicBezire = new(points, cubicBezireStyle);

        if (attachedPoints is not null)
        {
            foreach (Point point in attachedPoints)
            {
                point.AttachTo(cubicBezire);
            }
        }

        _figures.Add(cubicBezire);
    }

    private void Clear()
    {
        _points.Clear();
        _figures.Clear();

        _jsonPointObjects?.Clear();
        _jsonFigureObjects?.Clear();
    }
}
