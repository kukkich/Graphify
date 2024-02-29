using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.IO.Exporters;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Graphify.Core.Model.IO.Export;

public class ExporterFactory : IExporterFactory
{
    private readonly Dictionary<ExportFileType, Func<IExporter>> _factoryMethods = [];

    public ExporterFactory(IServiceProvider serviceProvider)
    {
        InitializeFactoryMethods(serviceProvider);
    }

    // TODO waiting for IO
    private void InitializeFactoryMethods(IServiceProvider serviceProvider)
    {
        _factoryMethods.Add(ExportFileType.Svg, () => new SVGExporter(serviceProvider.GetRequiredService<ILogger<SVGExporter>>()));
        _factoryMethods.Add(ExportFileType.Png, () => new SVGExporter(serviceProvider.GetRequiredService<ILogger<SVGExporter>>()));
        _factoryMethods.Add(ExportFileType.Custom, () => new SVGExporter(serviceProvider.GetRequiredService<ILogger<SVGExporter>>()));
    }

    public IExporter CreateExporter(ExportFileType type)
    {
        return _factoryMethods[type]();
    }
}
