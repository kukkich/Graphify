using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class AddFigureCommand
{
    private readonly ApplicationContext _context;
    private readonly IFigure _figure;

    public AddFigureCommand(ApplicationContext context, IFigure point)
    {
        _context = context;
        _figure = point;
    }

    public void Execute()
    {
        _context.Surface.AddFigure(_figure);
    }

    public void Undo()
    {
        _context.Surface.TryRemove(_figure);
    }
}
