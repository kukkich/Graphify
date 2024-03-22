using Graphify.Client.Model.Geometry;

namespace Graphify.Client.Model.Commands;

public class ClearCommand : ICommand
{
    private readonly ApplicationContext _context;
    private Surface _surface = null!;

    public ClearCommand(ApplicationContext context)
    {
        _context = context;
    }

    public void Execute()
    {
        _surface = _context.Surface;
        _context.Surface.Clear();
    }

    public void Undo()
    {
        _context.SetSurface(_surface);
    }
}
