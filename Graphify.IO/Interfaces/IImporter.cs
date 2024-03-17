using Graphify.IO.Importers;

namespace Graphify.IO.Interfaces;

public interface IImporter
{
    public ImportResult ImportFrom(string path);
}
