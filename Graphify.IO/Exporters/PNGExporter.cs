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

    public void Export(IGeometryContext context, string path)
    {
        string svgPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + "_temporary.svg");

        _svgExporter.Export(context, svgPath);

        ConvertSvgToPng(path, svgPath);

        File.Delete(svgPath);
    }

    private static void ConvertSvgToPng(string path, string svgPath)
    {
        using var document = new SVGDocument(svgPath);
        var pngSaveOptions = new ImageSaveOptions();

        Converter.ConvertSVG(document, pngSaveOptions, path);
    }
}
