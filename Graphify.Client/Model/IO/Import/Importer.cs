using Graphify.Client.Model;
using Graphify.Client.Model.Enums;
using Graphify.IO.Importers;
using Graphify.IO.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Graphify.Core.Model.IO.Import;

public class Importer
{
    private readonly ApplicationContext _context;
    private readonly Dictionary<ImportFileType, IImporter> _importers = [];

    public Importer(IServiceProvider serviceProvider)
    {
        _context = serviceProvider.GetRequiredService<ApplicationContext>();

        _importers.Add(ImportFileType.Custom, serviceProvider.GetRequiredService<GraphifyImporter>());
    }

    public SaveResult Import(ImportFileType fileType, string path)
    {
        ImportResult importResult = _importers[fileType].ImportFrom(path);

        foreach (var figure in importResult.Figures)
        {
            _context.Surface.AddObject(figure);
        }

        foreach (var point in importResult.Points)
        {
            _context.Surface.AddObject(point);
        }

        return SaveResult.Success;
    }
}
