using Aspose.Svg;
using Aspose.Svg.Converters;
using Aspose.Svg.Saving;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.IO.Interfaces;

namespace Graphify.IO.Exporters;

public sealed class PNGExporter : IExporter
{
    private readonly SVGExporter _svgExporter;

    public PNGExporter(SVGExporter svgExporter)
    {
        _svgExporter = svgExporter;
    }

    public void Export(IGeometryContext context, string pathPNG)
    {
        string pathSVG = Path.ChangeExtension(pathPNG, ".svg");

        _svgExporter.Export(context, pathSVG);

        ConvertSvgToPng(pathPNG, pathSVG);

        File.Delete(pathSVG);
    }

    private static void ConvertSvgToPng(string path, string svgPath)
    {
        using var document = new SVGDocument(svgPath);
        var pngSaveOptions = new ImageSaveOptions();
        
        Converter.ConvertSVG(document, pngSaveOptions, path);
    }
}
