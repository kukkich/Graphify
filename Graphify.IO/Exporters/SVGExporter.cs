using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using Aspose.Svg;
using Aspose.Svg.Builder;
using System.Numerics;
using Graphify.Geometry.GeometricObjects.Curves;

namespace Graphify.IO.Exporters;

public sealed class SVGExporter : IExporter
{
    private readonly ILogger<SVGExporter> _logger;

    private readonly SVGSVGElementBuilder _svgElements = new();

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
            case ObjectType.Line: AddLine(data, controlPoints.ToList<Point>()); break;
            case ObjectType.Circle: AddCircle(data, controlPoints.ToList<Point>()); break;
            case ObjectType.Polygon: AddPolygon(); break;
            case ObjectType.CubicBezier: AddCubicBezier(data, controlPoints.ToList<Point>()); break;
        }
    }

    private void AddPoint(PointExportData dataPoint)
    {
        _svgElements.AddCircle(
            circle => circle
                .Cx(dataPoint.Position.X)
                .Cy(dataPoint.Position.Y)
                .R(dataPoint.Style.Size)
                .Fill(dataPoint.Style.PrimaryColor)
                // .FillOpacity(1) // Прозрачность заливки 
                );
    }

    private void AddLine(FigureExportData dataLine, List<Point> points)
    {
        if (points.Count != 2)
        {
            _logger.LogError(
                "The number of points to build a line is not equal to 2. The number of points contained: {pointsCount}", points.Count);

            throw new ArgumentException("");
        }
        
        _svgElements.AddLine(
            line => line
                .X1(points[0].X)
                .Y1(points[0].Y)
                .X2(points[1].X)
                .Y2(points[1].Y)
                .Stroke(dataLine.Style.PrimaryColor)
                .StrokeWidth((dataLine.Style as CurveStyle ?? CurveStyle.Default).Size));
    }
    
    private void AddCircle(FigureExportData dataCircle, List<Point> points)
    {
        if (points.Count != 2)
        {
            _logger.LogError(
                "The number of points to build a circle is not equal to 2. The number of points contained: {pointsCount}", points.Count);

            throw new ArgumentException("");
        }

        List<Vector2> circlePoints =
        [
            new Vector2 { X = points[0].X, Y = points[0].Y },
            new Vector2 { X = points[1].X, Y = points[1].Y }
        ];

        float distance = Vector2.Distance(circlePoints[0], circlePoints[1]);

        _svgElements.AddCircle(
            circle => circle
                .Cx(circlePoints[0].X)
                .Cy(circlePoints[0].Y)
                .R(distance)
                .Fill(Paint.None)
                .Stroke(dataCircle.Style.PrimaryColor)
                .StrokeWidth((dataCircle.Style as CurveStyle ?? CurveStyle.Default).Size)
            );
    }

    private void AddPolygon() => throw new NotImplementedException();

    private void AddPolyline() => throw new NotImplementedException();

    private void AddCubicBezier(FigureExportData dataBezier, List<Point> points)
    {
        if (points.Count != 4)
        {
            _logger.LogError(
                "The number of points to build a cubic bezier is not equal to 4. The number of points contained: {pointsCount}", points.Count);

            throw new ArgumentException("");
        }

        _svgElements.AddPath(
            cubicBezier => cubicBezier
                .D(
                d => d
                    .M(points[0].X, points[0].Y)
                    .C(points[1].X, points[1].Y, points[2].X, points[2].Y, points[3].X, points[3].Y)
                    .Z())
                .Fill(Paint.None)
                .Stroke(dataBezier.Style.PrimaryColor)
                .StrokeWidth((dataBezier.Style as CurveStyle ?? CurveStyle.Default).Size));
    }

    private void CreateFile(string path)
    { 
        // Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        using var document = new SVGDocument();

        _svgElements
            .Width(SvgWidth < 1f ? 800D : SvgWidth)
            .Height(SvgHeight < 1f ? 800D : SvgHeight)
            .Build(document.FirstChild as SVGSVGElement);

        document.Save(path);
    }
}
