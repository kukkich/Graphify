using Graphify.Client.Model;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.IO.Exporters;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Graphify.Core.Model.IO.Export;

public class Exporter
{
    private readonly ApplicationContext _context;
    private readonly Dictionary<ExportFileType, IExporter> _exporters = [];

    public Exporter(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<ApplicationContext>();
        _exporters.Add(ExportFileType.Svg, serviceProvider.GetRequiredService<SVGExporter>());
        _exporters.Add(ExportFileType.Png, serviceProvider.GetRequiredService<PNGExporter>());
        _exporters.Add(ExportFileType.Custom, serviceProvider.GetRequiredService<GraphifyExporter>());
    }

    public SaveResult Export(ExportFileType fileType, string path)
    {
        _exporters[fileType].Export(_context.Surface, path);
        return SaveResult.Success;
    }
}
