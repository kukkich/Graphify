using Graphify.Client.Enums;
using Graphify.Core.Geometry;
using Graphify.IO;

namespace Graphify.Core.IO;

public class Exporter
{
    private readonly Dictionary<ExportFileType, IExporter> _exporters = [];
    
    public Exporter(IPNGExporter pngExporter, ISVGExporter svgExporter)
    {
        _exporters.Add(ExportFileType.Png, pngExporter);
        _exporters.Add(ExportFileType.Svg, svgExporter);
    }

    public SaveResult Export(string path, Surface context)
    {
        return SaveResult.UnknownError;
    }
}
