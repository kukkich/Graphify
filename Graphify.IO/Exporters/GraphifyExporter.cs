using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Graphify.IO.Exporters;

public class GraphifyExporter(ILogger<GraphifyExporter> logger) : IExporter
{
    private readonly ILogger<GraphifyExporter> _logger = logger;

    private readonly List<JsonPointObject> _points = [];
    private readonly List<JsonFigureObject> _figures = [];

    private readonly Dictionary<Point, uint> _completedPoints = [];

    public void Export(IGeometryContext context, string path)
    {
        uint id = 1;

        List<Point>? independentPoints = context.Points.Where(p => !p.IsAttached).ToList();

        if (independentPoints is not null)
        {
            AddPoints(independentPoints, ref id);
        }

        foreach (IFigure figure in context.Figures)
        {
            FigureExportData exportFigureData = figure.GetExportData();

            uint[] idControlPoints = AddPoints(figure.ControlPoints.ToList(), ref id);
            uint[] idAttachedPoints = AddPoints(figure.Attached.ToList(), ref id);

            _figures.Add(new JsonFigureObject(
                                             exportFigureData.FigureType,
                                             idAttachedPoints,
                                             idControlPoints,
                                             exportFigureData.Style));
        }

        CreateFile(path);

        _logger.LogDebug("Successfully export to gfy!");

        _points.Clear();
        _figures.Clear();
    }

    private uint[] AddPoints(List<Point> points, ref uint id)
    {
        List<uint> result = [];

        foreach (Point point in points)
        {
            if (_completedPoints.TryGetValue(point, out uint idPoint))
            {
                result.Add(idPoint);
                continue;
            }

            PointExportData data = point.GetExportData();

            result.Add(id);

            _points.Add(new JsonPointObject(id, data.Position, data.Style));
            _completedPoints.Add(point, id);

            ++id;
        }

        return [.. result];
    }

    private void CreateFile(string path)
    {
        string json = JsonConvert.SerializeObject((_points, _figures), Formatting.Indented);

        File.WriteAllText(path, json);
    }
}
