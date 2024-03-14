using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Aspose.Svg;
using Aspose.Svg.Builder;
using Aspose.Svg.Converters;
using Aspose.Svg.Saving;
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
        private readonly SVGExporter _svgExporter; 

        public PNGExporter(SVGExporter svgExporter)
        {
            _svgExporter = svgExporter; 
        }

        public void Export(IGeometryContext context, string path)
        {
            _svgExporter.Export(context, path);

            ConvertSvgToPng(path);
        }

        private static void ConvertSvgToPng(string svgPath)
        {
            using (var document = new SVGDocument(svgPath))
            {
                var pngSaveOptions = new ImageSaveOptions();

                Converter.ConvertSVG(document, pngSaveOptions, Path.ChangeExtension(svgPath, ".png"));
            }
        }
    }
}
