using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing; 
using System.Drawing.Imaging;
using System.Numerics;
using Graphify.Geometry.GeometricObjects.Curves;

namespace Graphify.IO.Exporters
{
    public sealed class PNGExporter : IExporter
    {
        private readonly ILogger<PNGExporter> _logger;

        private Bitmap _bitmap; 

        private float _pngWidth = 0f; 
        private float _pngHeight = 0f; 

        public PNGExporter(ILogger<PNGExporter> logger)
        {
            _logger = logger;
        }

        public void Export(IGeometryContext context, string path)
        {
            CalculateDimensions(context.Points);  

            _bitmap = new Bitmap((int)_pngWidth, (int)_pngHeight); 

            using (Graphics g = Graphics.FromImage(_bitmap))
            {
                g.Clear(Color.White); 

                IEnumerable<Point> points = context.Points;
                IEnumerable<IFigure> figures = context.Figures;

                foreach (Point point in points)
                {
                    PointExportData pointExportData = point.GetExportData();
                    DrawPoint(g, pointExportData);
                }

                foreach (IFigure figure in figures)
                {
                    FigureExportData figureExportData = figure.GetExportData();
                    DrawFigure(g, figureExportData, figure.ControlPoints);
                }
            }

            SaveBitmap(path);
        }

        private void CalculateDimensions(IEnumerable<Point> points)
        {
            foreach (Point point in points)
            {
                if (point.Position.X > _pngWidth)
                    _pngWidth = point.Position.X;
                if (point.Position.Y > _pngHeight)
                    _pngHeight = point.Position.Y;
            }
        }


        private void DrawPoint(Graphics graphics, PointExportData dataPoint)
        {
            float size = dataPoint.Style.Size;
            Color color = dataPoint.Style.PrimaryColor;
            PointCoordinates position = new PointCoordinates(dataPoint.Position.X, dataPoint.Position.Y);

            using (Brush brush = new SolidBrush(color))
            {
                graphics.FillEllipse(brush, position.X - size / 2, position.Y - size / 2, size, size);
            }
        }

        private void DrawFigure(Graphics g, FigureExportData data, IEnumerable<Point> controlPoints)
        {
            switch (data.FigureType)
            {
                case ObjectType.Line:
                    DrawLine(g, data, controlPoints);
                    break;

                case ObjectType.Circle:
                    
                    break;

                case ObjectType.Polygon:
                    break;

                case ObjectType.CubicBezier:
                    break;
            }
        }


        private void DrawLine(Graphics graphics, FigureExportData dataLine, IEnumerable<Point> points)
        {
            if (points == null || points.Count() != 2)
            {
                _logger.LogError("Invalid number of points to draw a line.");
                return;
            }

            PointCoordinates point1 = new PointCoordinates(points.ElementAt(0).X, points.ElementAt(0).Y);
            PointCoordinates point2 = new PointCoordinates(points.ElementAt(1).X, points.ElementAt(1).Y);

            using (Pen pen = new Pen(dataLine.Style.PrimaryColor, (dataLine.Style as CurveStyle ?? CurveStyle.Default).Size))
            {
                graphics.DrawLine(pen, point1, point2);
            }
        }

        private void DrawCircle(Graphics graphics, FigureExportData dataCircle, IEnumerable<Point> points)
        {
            if (points == null || points.Count() != 2)
            {
                _logger.LogError("Invalid number of points to draw a circle.");
                return;
            }

            float radius = Vector2.Distance(new Vector2(points.ElementAt(0).X, points.ElementAt(0).Y), new Vector2(points.ElementAt(1).X, points.ElementAt(1).Y));

            PointCoordinates center = new PointCoordinates(points.ElementAt(0).X - radius, points.ElementAt(0).Y - radius);
            RectangleF rect = new RectangleF(center, new SizeF(radius * 2, radius * 2));

            using (Pen pen = new Pen(dataCircle.Style.PrimaryColor, (dataCircle.Style as CurveStyle ?? CurveStyle.Default).Size))
            {
                graphics.DrawEllipse(pen, rect);
            }
        }

        private void DrawCubicBezier(Graphics graphics, FigureExportData dataBezier, IEnumerable<Point> points)
        {
            if (points == null || points.Count() != 4)
            {
                _logger.LogError("Invalid number of points to draw a cubic bezier.");
                return;
            }

            PointCoordinates[] bezierPoints = new PointCoordinates[]
            {
                new PointCoordinates(points.ElementAt(0).X, points.ElementAt(0).Y),
                new PointCoordinates(points.ElementAt(1).X, points.ElementAt(1).Y),
                new PointCoordinates(points.ElementAt(2).X, points.ElementAt(2).Y),
                new PointCoordinates(points.ElementAt(3).X, points.ElementAt(3).Y)
            };

            using (Pen pen = new Pen(dataBezier.Style.PrimaryColor, (dataBezier.Style as CurveStyle ?? CurveStyle.Default).Size))
            {
                graphics.DrawBezier(pen, bezierPoints[0], bezierPoints[1], bezierPoints[2], bezierPoints[3]);
            }
        }


        private void SaveBitmap(string path)
        {
            _bitmap.Save(path, ImageFormat.Png);
        }
    }
}
