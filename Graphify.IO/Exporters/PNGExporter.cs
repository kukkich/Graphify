using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Aspose.Imaging.ImageOptions;
using Aspose.Svg;
using Aspose.Svg.Builder;
using DynamicData;
using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Polygons;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;

namespace Graphify.IO.Exporters
{
    public sealed class PNGExporter : IExporter
    {
        private readonly ILogger<PNGExporter> _logger;

        public PNGExporter(ILogger<PNGExporter> logger)
        {
            _logger = logger;
        }

        public void Export(IGeometryContext context, string path)
        {
            var svgDocument = CreateSvgDocument(context);

            var tempSvgPath = Path.GetTempFileName();
            svgDocument.Save(tempSvgPath);

            using (var image = Aspose.Imaging.Image.Load(tempSvgPath))
            {
                var pngOptions = new PngOptions();
                image.Save(path, pngOptions);
            }
        }

        private SVGDocument CreateSvgDocument(IGeometryContext context)
        {
            var svgElements = new SVGSVGElementBuilder();
            IEnumerable<Point> points = context.Points;
            IEnumerable<IFigure> figures = context.Figures;

            foreach (Point point in points)
            {
                PointExportData pointExportData = point.GetExportData();
                AddPoint(svgElements, pointExportData);
            }

            foreach (IFigure figure in figures)
            {
                FigureExportData figureExportData = figure.GetExportData();
                AddFigure(svgElements, figureExportData, figure.ControlPoints);
            }

            var svgDocument = new SVGDocument();
            svgElements.Build(svgDocument.FirstChild as SVGSVGElement);

            return svgDocument;
        }

        private void AddPoint(SVGSVGElementBuilder svgElements, PointExportData dataPoint)
        {
            svgElements.AddCircle(
                circle => circle
                    .Cx(dataPoint.Position.X)
                    .Cy(dataPoint.Position.Y)
                    .R(dataPoint.Style.Size)
                    .Fill(dataPoint.Style.PrimaryColor)
                    .Transform(t => t.Scale(1, -1))
                    );
        }

        private void AddFigure(SVGSVGElementBuilder svgElements, FigureExportData data, IEnumerable<Point> controlPoints)
        {
            switch (data.FigureType)
            {
                case ObjectType.Line: AddLine(svgElements, data, controlPoints.ToList<Point>()); break;
                case ObjectType.Circle: AddCircle(svgElements, data, controlPoints.ToList<Point>()); break;
                case ObjectType.Polygon: AddPolygon(svgElements, data, controlPoints.ToList<Point>()); break;
                case ObjectType.CubicBezier: AddCubicBezier(svgElements, data, controlPoints.ToList<Point>()); break;
            }
        }

        private void AddLine(SVGSVGElementBuilder svgElements, FigureExportData dataLine, List<Point> points)
        {
            if (points.Count != 2)
            {
                _logger.LogError(
                    "The number of points to build a line is not equal to 2. The number of points contained: {pointsCount}", points.Count);

                throw new ArgumentException("");
            }

            svgElements.AddLine(
                line => line
                    .X1(points[0].X)
                    .Y1(points[0].Y)
                    .X2(points[1].X)
                    .Y2(points[1].Y)
                    .Stroke(dataLine.Style.PrimaryColor)
                    .StrokeWidth((dataLine.Style as CurveStyle ?? CurveStyle.Default).Size)
                    .Transform(t => t.Scale(1, -1))
                    );
        }

        private void AddCircle(SVGSVGElementBuilder svgElements, FigureExportData dataCircle, List<Point> points)
        {
            if (points.Count != 2)
            {
                _logger.LogError(
                    "The number of points to build a circle is not equal to 2. The number of points contained: {pointsCount}", points.Count);

                throw new ArgumentException("");
            }

            var circlePoints = points.ToVector2();

            float radius = Vector2.Distance(circlePoints[0], circlePoints[1]);

            svgElements.AddCircle(
                circle => circle
                    .Cx(circlePoints[0].X)
                    .Cy(circlePoints[0].Y)
                    .R(radius)
                    .Fill(Paint.None)
                    .Stroke(dataCircle.Style.PrimaryColor)
                    .StrokeWidth((dataCircle.Style as CurveStyle ?? CurveStyle.Default).Size)
                    .Transform(t => t.Scale(1, -1))
                );
        }

        private void AddPolygon(SVGSVGElementBuilder svgElements, FigureExportData dataPolygon, List<Point> points)
        {
            if (points.Count < 3)
            {
                _logger.LogError(
                    "The number of points to build a Polygon less 3. The number of points contained: {pointsCount}", points.Count);

                throw new ArgumentException("");
            }

            double[] pointsSVG = points.SelectMany(x => x.PointToArray()).ToArray();

            svgElements.AddPolygon(
                polygon => polygon
                    .Points(pointsSVG)
                    .Fill(dataPolygon.Style.PrimaryColor)
                    .Stroke(Paint.None)
                    .Transform(t => t.Scale(1, -1))
                );
        }

        private void AddCubicBezier(SVGSVGElementBuilder svgElements, FigureExportData dataBezier, List<Point> points)
        {
            if (points.Count != 4)
            {
                _logger.LogError(
                    "The number of points to build a cubic bezier is not equal to 4. The number of points contained: {pointsCount}", points.Count);

                throw new ArgumentException("");
            }

            svgElements.AddPath(
                cubicBezier => cubicBezier
                    .D(
                    d => d
                        .M(points[0].X, points[0].Y)
                        .C(points[1].X, points[1].Y, points[2].X, points[2].Y, points[3].X, points[3].Y)
                        .Z())
                    .Fill(Paint.None)
                    .Stroke(dataBezier.Style.PrimaryColor)
                    .StrokeWidth((dataBezier.Style as CurveStyle ?? CurveStyle.Default).Size)
                    .Transform(t => t.Scale(1, -1))
                );
        }
    }
}
