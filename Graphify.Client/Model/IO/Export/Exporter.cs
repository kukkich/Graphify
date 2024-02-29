using Graphify.Client.Model;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.IO.Interfaces;

namespace Graphify.Core.Model.IO.Export;

public class Exporter
{
    private readonly Application _application;
    private readonly Dictionary<ExportFileType, IExporter> _exporters = [];
    
    public Exporter(Application application, IExporterFactory exporterFactory)
    {
        _application = application;
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
        _exporters[fileType].Export(_application.Context.Surface, path);
        return SaveResult.Success;
    }
}
