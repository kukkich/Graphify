using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class DeleteCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly IGeometricObject _geometricObject;

    public DeleteCommand(ApplicationContext context, IGeometricObject geometricObject)
    {
        _context = context;
        _geometricObject = geometricObject;
    }

    public void Execute()
    {
        _context.Surface.TryRemove(_geometricObject);
    }

    public void Undo()
    {
        _context.Surface.AddObject(_geometricObject);
    }
}
