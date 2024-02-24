namespace Graphify.Core;

public class GraphifyApplication
{
    public ApplicationContext Context { get; private set; }
    
    public GraphifyApplication()
    {
    }
    
    static GraphifyApplication Instance()
    {
        return null;
    }

    void LoadContext(ApplicationContext context)
    {
        Context = context;
    }
}
