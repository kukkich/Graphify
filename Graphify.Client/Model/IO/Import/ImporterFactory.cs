using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.IO.Interfaces;

namespace Graphify.Core.Model.IO.Import;

public class ImporterFactory : IImporterFactory
{
    private readonly Dictionary<ImportFileType, Func<IImporter>> _factoryMethods = [];

    public ImporterFactory(IServiceProvider serviceProvider)
    {
        InitializeFactoryMethods(serviceProvider);
    }

    private void InitializeFactoryMethods(IServiceProvider serviceProvider)
    {
    }

    public IImporter CreateImporter(ImportFileType type)
    {
        return _factoryMethods[type]();
    }
}
