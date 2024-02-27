using Graphify.Core.Geometry;

namespace Graphify.Core;

public class ApplicationContext
{
    public Surface Surface { get; private set; } 
    
    public ApplicationContext(Surface surface)
    {
        Surface = surface;
    }
}
