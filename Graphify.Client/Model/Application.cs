using Graphify.Client.Model.Commands;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model;

public class Application
{
    private readonly IGeometryFactory _geometryFactory;
    private readonly ApplicationContext _context;
    private readonly CommandsBuffer _commandsBuffer;

    public Application(IGeometryFactory geometryFactory, ApplicationContext context, CommandsBuffer commandsBuffer)
    {
        _geometryFactory = geometryFactory;
        _context = context;
        _commandsBuffer = commandsBuffer;
    }

    public void AddPoint(Point point)
    {
        _context.Surface.AddPoint(point);
        _commandsBuffer.AddCommand(new AddPointCommand(_context, point));
    }

    public void UndoAction()
    {
        _commandsBuffer.Undo();
    }

    public void RedoAction()
    {
        _commandsBuffer.Redo();
    }
}
