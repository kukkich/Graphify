using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class DeleteCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly HashSet<IGeometricObject> _geometricObjects;

    public DeleteCommand(ApplicationContext context, HashSet<IGeometricObject> geometricObjects)
    {
        _context = context;
        _geometricObjects = geometricObjects;
    }

    public void Execute()
    {
        foreach (var geometricObject in _geometricObjects)
        {
            _context.Surface.TryRemove(geometricObject);
        }

    }

    public void Undo()
    {
        foreach (var geometricObject in _geometricObjects)
        {
            _context.Surface.AddObject(geometricObject);
        }
    }
}
