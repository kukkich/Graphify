using Graphify.Client.Enums;
using Graphify.Client.Model.Geometry;
using Graphify.IO.Exporters;
using Graphify.IO.Interfaces;

namespace Graphify.Core.IO;

public class Exporter
{
    private readonly Dictionary<ExportFileType, IExporter> _exporters = [];
    
    public Exporter(PNGExporter pngExporter, SVGExporter svgExporter)
    {
        _exporters.Add(ExportFileType.Png, pngExporter);
        _exporters.Add(ExportFileType.Svg, svgExporter);
    }

    public SaveResult Export(string path, Surface context)
    {
        return SaveResult.UnknownError;
    }
}
