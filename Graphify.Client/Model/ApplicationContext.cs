using Graphify.Client.Model.Geometry;

namespace Graphify.Client.Model;

public class ApplicationContext
{
    public Surface Surface { get; private set; }

    public ApplicationContext(Surface surface)
    {
        Surface = surface;
    }
}
