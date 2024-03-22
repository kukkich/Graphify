using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.IO.Interfaces;
using Svg;

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
        byte[] byteArray = Encoding.ASCII.GetBytes(svgPath);

        using var stream = new MemoryStream(byteArray);

        SvgDocument svgDocument = SvgDocument.Open(svgPath);

        Bitmap bitmap = svgDocument.Draw();

        bitmap.Save(path, ImageFormat.Png);
    }
}
