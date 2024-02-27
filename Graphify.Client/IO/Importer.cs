using Graphify.Client.Enums;
using Graphify.Client.Model.Geometry;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.IO;

namespace Graphify.Core.IO;

public class Importer
{
    private readonly Dictionary<ImportFileType, IImporter> _importers = [];
    
    public Importer(/*ISvgImporter svgImporter*/)
    {
        /*_importers.Add(ImportFileType.Png, svgImporter);*/
    }

    public Surface? Import(string path, IGeometryFactory factory)
    {
        throw new NotImplementedException();
    }
}
