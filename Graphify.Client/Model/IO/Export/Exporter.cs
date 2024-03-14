using Graphify.Client.Model;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.IO.Interfaces;

namespace Graphify.Core.Model.IO.Export;

public class Exporter
{
    private readonly ApplicationContext _context;
    private readonly Dictionary<ExportFileType, IExporter> _exporters = [];

    public Exporter(ApplicationContext context, IExporterFactory exporterFactory)
    {
        _context = context;
        CreateExporters(exporterFactory);
    }

    private void CreateExporters(IExporterFactory exporterFactory)
    {
        foreach (ExportFileType type in Enum.GetValues(typeof(ExportFileType)))
        {
            _exporters.Add(type, exporterFactory.CreateExporter(type));
        }
    }

    public SaveResult Export(ExportFileType fileType, string path)
    {
        _exporters[fileType].Export(_context.Surface, path);
        return SaveResult.Success;
    }
}
