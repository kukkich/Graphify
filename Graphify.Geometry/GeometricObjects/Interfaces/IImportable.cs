using Graphify.Geometry.GeometricObjects.Interfaces.Public;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IImportable
{
    public void Import(IImporter importer);
}
