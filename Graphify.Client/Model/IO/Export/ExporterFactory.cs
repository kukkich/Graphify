using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.IO.Exporters;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Graphify.Core.Model.IO.Export;

public class ExporterFactory : IExporterFactory
{
    private readonly Dictionary<ExportFileType, Func<IExporter>> _factoryMethods = [];

    public ExporterFactory(IServiceProvider serviceProvider)
    {
        InitializeFactoryMethods(serviceProvider);
    }

    private void InitializeFactoryMethods(IServiceProvider serviceProvider)
    {
        _factoryMethods.Add(ExportFileType.Svg, serviceProvider.GetRequiredService<SVGExporter>);
        _factoryMethods.Add(ExportFileType.Png, serviceProvider.GetRequiredService<PNGExporter>);
        _factoryMethods.Add(ExportFileType.Custom, serviceProvider.GetRequiredService<GraphifyExporter>);
    }

    public IExporter CreateExporter(ExportFileType type)
    {
        return _factoryMethods[type]();
    }
}
