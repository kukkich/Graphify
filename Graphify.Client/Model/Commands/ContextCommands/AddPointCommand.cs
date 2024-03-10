using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Commands;

public class AddPointCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly Point _point;

    public AddPointCommand(ApplicationContext context, Point point)
    {
        _context = context;
        _point = point;
    }

    public void Execute()
    {
        _context.Surface.AddPoint(_point);
    }

    public void Undo()
    {
        _context.Surface.TryRemove(_point);
    }
}
