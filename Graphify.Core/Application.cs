using Graphify.Core.Geometry;
using Graphify.Core.IO;

namespace Graphify.Core;

public class Application
{
    public ApplicationContext ApplicationContext { get; }
    public PersistenceManager PersistenceManager { get; }
    
    public Application(ApplicationContext applicationContext, PersistenceManager persistenceManager)
    {
        ApplicationContext = applicationContext;
        PersistenceManager = persistenceManager;
    }
}
