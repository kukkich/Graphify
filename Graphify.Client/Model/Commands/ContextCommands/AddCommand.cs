using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class AddCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly IGeometricObject _geometricObject;

    public AddCommand(ApplicationContext context, IGeometricObject geometricObject)
    {
        _context = context;
        _geometricObject = geometricObject;
    }

    public void Execute()
    {
        _context.Surface.AddObject(_geometricObject);
    }

    public void Undo()
    {
        _context.Surface.TryRemove(_geometricObject);
    }
}
