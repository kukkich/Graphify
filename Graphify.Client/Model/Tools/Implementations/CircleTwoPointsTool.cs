using System.Numerics;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class CircleTwoPointsTool : IApplicationTool
{
    private readonly ApplicationContext _context;
    private readonly CommandsBuffer _commandsBuffer;

    private const int RequiredClicks = 1;
    private int _currentClicks = 0;
    private Point? _firstPoint;
    private Point? _secondPoint;
    public CircleTwoPointsTool(ApplicationContext context, CommandsBuffer commandsBuffer)
    {
        _context = context;
        _commandsBuffer = commandsBuffer;
    }

    public void MouseMove(Vector2 newPosition)
    {

    }

    public void MouseDown(Vector2 clickPosition)
    {
        if (_currentClicks < RequiredClicks)
        {
            _firstPoint = _context.CreatePoint(clickPosition);
            ++_currentClicks;
        }
        else
        {
            _secondPoint = _context.CreatePoint(clickPosition);
            IFigure circle = _context.CreateFigure(ObjectType.Circle, [_firstPoint, _secondPoint]);
            _commandsBuffer.AddCommand(new AddCommand(_context, circle));
            Reset();
        }
    }

    public void Cancel()
    {

    }

    public void Reset()
    {
        _currentClicks = 0;
        _firstPoint = null;
        _secondPoint = null;
    }
}
