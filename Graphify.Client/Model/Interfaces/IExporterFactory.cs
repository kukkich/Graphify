using Graphify.Client.Model.Enums;
using Graphify.IO.Interfaces;

namespace Graphify.Client.Model.Interfaces;

public interface IExporterFactory
{
    IExporter CreateExporter(ExportFileType type);
}
