using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class DeleteCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly IFigure _figure;

    public DeleteCommand(ApplicationContext context, IFigure figure)
    {
        _context = context;
        _figure = figure;
    }

    public void Execute()
    {
        _context.Surface.TryRemove(_figure);

        foreach (var point in _figure.ControlPoints)
        {
            _context.Surface.TryRemove(point);
        }
    }

    public void Undo()
    { 
        _context.Surface.AddFigure(_figure);

        foreach (var point in _figure.ControlPoints)
        {
            _context.Surface.AddPoint(point);
        }
    }
}
