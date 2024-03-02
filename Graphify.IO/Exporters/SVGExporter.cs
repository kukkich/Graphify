using System.Numerics;
using Aspose.Svg;
using Aspose.Svg.Builder;
using Aspose.Svg.Toolkit.Optimizers;
using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using System.Linq;

namespace Graphify.IO.Exporters;

public sealed class SVGExporter(ILogger<SVGExporter> logger) : IExporter
{
    private const int ExtraSize = 50;

    private readonly ILogger<SVGExporter> _logger = logger;

    private SVGSVGElementBuilder _svgElements = null!;

    private Vector2 LeftBottomBound { get; set; }

    private Vector2 RightTopBound { get; set; }

    public void Export(IGeometryContext context, string path)
    {
        _svgElements = new();

        IEnumerable<Point> points = context.Points;
        IEnumerable<IFigure> figures = context.Figures;

        foreach (Point point in points)
        {
            PointExportData pointExportData = point.GetExportData();
            ExportPoint(pointExportData);

            UpdateSvgSize(pointExportData);
        }

        foreach (IFigure figure in figures)
        {
            FigureExportData figureExportData = figure.GetExportData();
            ExportFigure(figureExportData, figure.ControlPoints);

            UpdateSvgSize(figureExportData);
        }

        CreateFile(path);
        DeleteWatermark(path);

        _logger.LogDebug("Successfully export to svg!");
    }

    private void ExportPoint(PointExportData data) => AddPoint(data);

    private void ExportFigure(FigureExportData data, IEnumerable<Point> controlPoints)
    {
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
                .Transform(t => t.Scale(1, -1))
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

        var circlePoints = points.ToListVector2();

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
        float distanceBorders = Vector2.Distance(LeftBottomBound, RightTopBound);

        using var document = new SVGDocument();

        _svgElements
            .ViewBox(
                LeftBottomBound.X - ExtraSize,
                -RightTopBound.Y - ExtraSize,
                distanceBorders,
                distanceBorders)
            .Build(document.FirstChild as SVGSVGElement);

        var options = new SVGOptimizationOptions
        {
            CleanListOfValues = true,
            RemoveUselessStrokeAndFill = true,
        };

        SVGOptimizer.Optimize(document, options);

        document.Save(path);
    }

    private void UpdateSvgSize(PointExportData data)
    {
        if (data.Position.X < LeftBottomBound.X)
        {
            LeftBottomBound = new Vector2(data.Position.X, LeftBottomBound.Y);
        }

        if (data.Position.Y < LeftBottomBound.Y)
        {
            LeftBottomBound = new Vector2(LeftBottomBound.X, data.Position.Y);
        }

        if (data.Position.X > RightTopBound.X)
        {
            RightTopBound = new Vector2(data.Position.X, RightTopBound.Y);
        }

        if (data.Position.Y > RightTopBound.Y)
        {
            RightTopBound = new Vector2(data.Position.X, data.Position.Y);
        }
    }

    private void UpdateSvgSize(FigureExportData figures) => throw new NotImplementedException();

    private static void DeleteWatermark(string path)
    {
        const string str = "<text y=\"15\" style=\"font-family:Times New Roman; font-size:15px; fill:red;\" >Evaluation Only. Created with Aspose.SVG. Copyright 2018-2024 Aspose Pty Ltd.</text>";

        int lastIndex = path.LastIndexOf(".");

        using StreamReader reader = new(path);
        using StreamWriter writer = new(path.Insert(lastIndex,"WW"));

        var content = reader.ReadLine();
        var newContent = content!.Replace(str, "");
        
        writer.WriteLine(newContent);
    }
}
