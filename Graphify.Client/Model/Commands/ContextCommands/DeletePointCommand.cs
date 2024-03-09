using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Commands;

public class DeletePointCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly Point _point;

    public DeletePointCommand(ApplicationContext context, Point point)
    {
        _context = context;
        _point = point;
    }

    public void Execute()
    {
        _context.Surface.TryRemove(_point);
    }

    public void Undo()
    {
        _context.Surface.AddPoint(_point);
    }
}
