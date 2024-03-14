using Graphify.Geometry.Export;
using System.Linq;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;

namespace Graphify.IO.Exporters;

public class GraphifyExporter(ILogger<GraphifyExporter> logger) : IExporter
{
    private readonly ILogger<GraphifyExporter> _logger = logger;
    private StringBuilder _jsonElements = null!;

    public void Export(IGeometryContext context, string path)
    {
        _jsonElements = new StringBuilder();
        
        IEnumerable<Point> points = context.Points;
        IEnumerable<IFigure> figures = context.Figures;

        foreach (IFigure figure in figures)
        {
            FigureExportData figureExportData = figure.GetExportData();
            
        }

        foreach (Point point in points)
        {
            PointExportData pointExportData = point.GetExportData();
            ExportPoint(pointExportData);
        }

        CreateFile(path);
    }

    private void ExportPoint(PointExportData data) => AddPoint(data);
    private void AddPoint(PointExportData dataPoint)
    {
        _jsonElements.Append(JsonConvert.SerializeObject(dataPoint));
    }
    private void CreateFile(string path)
    {
        File.WriteAllText(path, _jsonElements.ToString());
    }
}
