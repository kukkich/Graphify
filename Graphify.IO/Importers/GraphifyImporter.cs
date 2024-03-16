using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Graphify.IO.Importers;

public partial class GraphifyImporter : IImporter
{
    private const byte ShiftToOpeningBracket = 8;

    private readonly ILogger<GraphifyImporter> _logger;

    private readonly List<Point> _points = [];

    private readonly List<IFigure> _figures = [];

    private List<JsonPointObject>? _jsonPointObjects = [];

    private List<JsonFigureObject>? _jsonFigureObjects = [];

    public GraphifyImporter(ILogger<GraphifyImporter> logger)
    {
        _logger = logger;
    }

    public ImportResult ImportFrom(string path)
    {
        ImportResult result = new ImportResult();

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
            Clear();

            _logger.LogDebug("Successfully import!");

            return new ImportResult() { Figures = _figures, Points = _points };
        }

        AddPoints();
        AddFigures();
        Clear();

        _logger.LogDebug("Successfully import!");

        return result;
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

    private Point? CreatePoint(uint id)
    {
        foreach (JsonPointObject point in _jsonPointObjects!)
        {
            if (point.Id == id)
            {
                float x = point.Position.X;
                float y = point.Position.Y;

                return new Point(x, y, point.Style);
            }
        }

        return null;
    }

    private List<Point>? CreateListPoints(uint[]? idPoints)
    {
        if (idPoints == null)
        {
            return null;
        }

        List<Point> points = [];

        foreach (uint idControlPoint in idPoints)
        {
            Point? point = CreatePoint(idControlPoint);

            if (point == null)
            {
                continue;
            }

            points.Add(point);
        }

        return points;
    }

    private void AddPoints()
    {
        foreach (JsonPointObject pointObject in _jsonPointObjects!)
        {
            _points.Add(new Point(pointObject.Position.X, pointObject.Position.Y, pointObject.Style));
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
            foreach (Point item in attachedPoints)
            {
                item.AttachTo(line);
            }
        }

        _figures.Add(line);
    }

    private void AddCircle(JsonFigureObject item) => throw new NotImplementedException();

    private void AddPolygon(JsonFigureObject item) => throw new NotImplementedException();

    private void AddCubicBezire(JsonFigureObject item) => throw new NotImplementedException();

    private void Clear()
    {
        _points.Clear();
        _figures.Clear();

        _jsonPointObjects?.Clear();
        _jsonFigureObjects?.Clear();
    }
}
