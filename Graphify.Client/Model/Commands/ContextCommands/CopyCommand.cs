using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

//TODO
public class CopyCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly IList<IGeometricObject> _objects;

    public CopyCommand(ApplicationContext context, IList<IGeometricObject> objects)
    {
        _context = context;
        _objects = objects;
    }

    public void Execute() => throw new NotImplementedException();

    public void Undo() => throw new NotImplementedException();
}
