using System.Numerics;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Draw;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model;

public class Application
{
    public ApplicationContext Context { get; }

    private readonly IGeometryFactory _geometryFactory;
    private readonly CommandsBuffer _commandsBuffer;
    private readonly DrawLoop _drawLoop;

    public Application(IGeometryFactory geometryFactory, ApplicationContext context, CommandsBuffer commandsBuffer, DrawLoop drawLoop)
    {
        Context = context;

        _geometryFactory = geometryFactory;
        _commandsBuffer = commandsBuffer;
        _drawLoop = drawLoop;

        _drawLoop.Initialize(160);
        _drawLoop.Start();
    }

    public void AddPoint(Vector2 pointCoords)
    {
        Point newPoint = _geometryFactory.Create(pointCoords);
        Context.Surface.AddPoint(newPoint);
        _commandsBuffer.AddCommand(new AddPointCommand(Context, newPoint));
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
