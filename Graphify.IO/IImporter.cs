namespace Graphify.IO;

public interface IImporter
{
    public ImportResult ImportFrom(string path);
}
