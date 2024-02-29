using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using Aspose.Svg;
using Aspose.Svg.Builder;
using System.Numerics;

namespace Graphify.IO.Exporters;

public sealed class SVGExporter : IExporter
{
    private readonly ILogger<SVGExporter> _logger;

    private readonly SVGSVGElementBuilder _svgElement = new();

    private float SvgWidth { get; set; } = 0f;
    private float SvgHeight { get; set; } = 0f;

    public SVGExporter(ILogger<SVGExporter> logger)
    {
        _logger = logger;
    }

    public void Export(IGeometryContext context, string path)
    {
        IEnumerable<Point> points = context.Points;
        IEnumerable<IFigure> figures = context.Figures;

        foreach (Point point in points)
        {
            PointExportData pointExportData = point.GetExportData();
            ExportPoint(pointExportData);
        }

        foreach (IFigure figure in figures)
        {
            FigureExportData figureExportData = figure.GetExportData();
            ExportFigure(figureExportData, figure.ControlPoints);
        }

        CreateFile(path);
    }

    private void ExportPoint(PointExportData data) => AddPoint(data);

    private void ExportFigure(FigureExportData data, IEnumerable<Point> controlPoints)
    {
        float rightTopBound = data.RightTopBound.X;
        float leftBottomBound = data.LeftBottomBound.Y;

        if (SvgHeight < leftBottomBound)
        {
            SvgHeight = leftBottomBound;
        }

        if (SvgWidth < rightTopBound)
        {
            SvgWidth = rightTopBound;
        }

        switch (data.FigureType)
        {
            case ObjectType.Line: AddLine(); break;
            case ObjectType.Circle: AddCircle(data, controlPoints.ToList<Point>()); break;
            case ObjectType.Polygon: AddPolygon(); break;
            case ObjectType.CubicBezier: AddCubicBezier(); break;
        }
    }

    private void AddPoint(PointExportData dataPoint)
    {
        _svgElement.AddCircle(
            circle => circle
                .Cx(dataPoint.Position.X)
                .Cy(dataPoint.Position.Y)
                .R(dataPoint.Style.Size)
                .Fill(dataPoint.Style.PrimaryColor)
                // .FillOpacity(1) // Прозрачность заливки 
                );
    }

    // TODO: Не сделано+
    private void AddLine()
    {
        _svgElement.AddLine(line => line
                           .X1(30)
                           .Y1(30)
                           .X2(350)
                           .Y2(290)
                           .Fill(System.Drawing.Color.Black)
                           .Stroke(System.Drawing.Color.Black));
    }

    private void AddCircle(FigureExportData dataCircle, List<Point> points)
    { 
        if (points.Count != 2)
        {
            _logger.LogDebug(
                "The number of points to build a circle is not equal to 2." +
                " The number of points contained: {}", points.Count);

            throw new ArgumentException("");
        }
        
        List<Vector2> circlePoints =
        [
            new Vector2() { X = points[0].X, Y = points[0].Y },
            new Vector2 { X = points[1].X, Y = points[1].X}
        ];

        float Distance = Vector2.Distance(circlePoints[0], circlePoints[1]);

        _svgElement.AddCircle( 
            circle => circle
                .Cx(circlePoints[0].X)
                .Cy(circlePoints[0].Y)
                .R(Distance)
                .Fill(Paint.None)
                .Stroke(dataCircle.Style.PrimaryColor)
            );
    }

    private void AddPolygon() => throw new NotImplementedException();

    private void AddPolyline() => throw new NotImplementedException();

    private void AddCubicBezier() => throw new NotImplementedException();

    private void CreateFile(string path)
    {
       // Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        using var document = new SVGDocument();

        _svgElement
            .Width(SvgWidth)
            .Height(SvgHeight)
            .Build(document.FirstChild as SVGSVGElement);

        document.Save(path);
    }
}
