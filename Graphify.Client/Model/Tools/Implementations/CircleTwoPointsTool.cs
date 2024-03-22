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

    private const int RequiredClicks = 2;
    private int _currentClicks = 0;

    private readonly List<Point> _points = [];

    public CircleTwoPointsTool(ApplicationContext context, CommandsBuffer commandsBuffer)
    {
        _context = context;
        _commandsBuffer = commandsBuffer;
    }

    public void RightMouseDown(Vector2 clickPosition) => throw new NotImplementedException();

    public void RightMouseUp(Vector2 clickPosition) => throw new NotImplementedException();

    public void MouseMove(Vector2 newPosition)
    {

    }

    public void MouseDown(Vector2 clickPosition)
    {
        _currentClicks++;
        IGeometricObject geometricObject = _context.Surface.TryGetClosestObject(clickPosition)!;

        Point newPoint;
        if (geometricObject is Point point)
        {
            newPoint = point;
        }
        else
        {
            newPoint = _context.CreatePoint(clickPosition);
        }
        _points.Add(newPoint);

        if (_currentClicks < RequiredClicks)
        {
            return;
        }

        IFigure circle = _context.CreateFigure(ObjectType.Circle, _points.ToArray());
        _commandsBuffer.AddCommand(new AddCommand(_context, circle));
        OnToolChanged();
    }

    public void MouseUp(Vector2 clickPosition)
    {

    }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel()
    {

    }

    public void OnToolChanged()
    {
        _currentClicks = 0;
        _points.Clear();
    }
}
