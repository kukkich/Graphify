using Graphify.Core.Geometry;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Core;

public class Application
{
    public IGeometryFactory Factory => _factory;
    public Surface Surface { get; private set; }

    private readonly GeometryFactory _factory;
    
    public Application(GeometryFactory factory)
    {
        _factory = factory;
        
        CreateEmptySurface();
    }

    public void CreateEmptySurface()
    {
        IGeometryContext newContext = new GeometryContext();

        Surface = new Surface(newContext);
        _factory.Context = newContext;
    }

    public void LoadSurface(Surface loadedSurface)
    {
        Surface = loadedSurface;
        _factory.Context = loadedSurface.GeometryContext;
    }
}
