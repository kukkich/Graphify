using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Reactive;

namespace Graphify.IO.Exporters;

public class GraphifyExporter(ILogger<GraphifyExporter> logger) : IExporter
{
    private readonly ILogger<GraphifyExporter> _logger = logger;

    private readonly List<JsonPointObject> _points = [];
    private readonly List<JsonFigureObject> _figures = [];

    private StringBuilder _jsonElements = null!;

    public void Export(IGeometryContext context, string path)
    {
        uint i = 1;

        _jsonElements = new StringBuilder();

        IEnumerable<Point> points = context.Points;
        IEnumerable<IFigure> figures = context.Figures;

        List<Point>? independentPoints = points.Where(p => !p.IsAttached).ToList();

        if (independentPoints is not null)
        {
            AddPoints(independentPoints, ref i);
        }

        foreach (IFigure figure in figures)
        {
            FigureExportData exportFigureData = figure.GetExportData();

            uint[] idControlPoints = AddPoints(figure.ControlPoints.ToList(), ref i);
            uint[] idAttachedPoints = AddPoints(figure.Attached.ToList(), ref i);

            _figures.Add(new JsonFigureObject(
                                             exportFigureData.FigureType,
                                             idAttachedPoints,
                                             idControlPoints,
                                             exportFigureData.Style));
        }

        CreateFile(path);

        _points.Clear();
    }

    private uint[] AddPoints(List<Point> points, ref uint i)
    {
        List<uint> result = [];

        foreach (Point point in points)
        {
            PointExportData data = point.GetExportData();

            _points.Add(new JsonPointObject(i, data.Position, data.Style));

            ++i;
        }

        return [.. result];
    }

    private void CreateFile(string path)
    {
        File.WriteAllText(path, _jsonElements.ToString());
    }
}
