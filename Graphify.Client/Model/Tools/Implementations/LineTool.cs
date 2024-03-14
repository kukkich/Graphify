using System.Numerics;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class LineTool : IApplicationTool
{
    private readonly ApplicationContext _context;
    private readonly CommandsBuffer _commandsBuffer;

    private const int RequiredClicks = 1;
    private int _currentClicks = 0;
    private Point? _firstPoint;
    private Point? _secondPoint;

    public LineTool(ApplicationContext context, CommandsBuffer commandsBuffer)
    {
        _context = context;
        _commandsBuffer = commandsBuffer;
    }

    public void RightMouseDown(Vector2 clickPosition) { }

    public void RightMouseUp(Vector2 clickPosition) { }

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
            IFigure line = _context.CreateFigure(ObjectType.Line, [_firstPoint, _secondPoint]);
            _commandsBuffer.AddCommand(new AddCommand(_context, line));
            OnToolChanged();
        }
    }

    public void MouseUp(Vector2 clickPosition)
    {
        
    }

    public bool InProgress()
    {
        return true;
    }

    public void Cancel()
    {
    }

    public void OnToolChanged()
    {
        _currentClicks = 0;
        _firstPoint = null;
        _secondPoint = null;
    }
}
