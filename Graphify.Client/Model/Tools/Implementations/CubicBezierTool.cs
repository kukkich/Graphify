using System.Numerics;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class CubicBezierTool : IApplicationTool
{
    private readonly ApplicationContext _context;
    private readonly CommandsBuffer _commandsBuffer;

    private readonly List<Point> _points = [];

    public CubicBezierTool(ApplicationContext context, CommandsBuffer commandsBuffer)
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
        var geometricObject = _context.Surface.TryGetClosestObject(clickPosition)!;

        Point newPoint;
        if (geometricObject is Point point)
        {
            newPoint = point;
        }
        else
        {
            newPoint = _context.CreatePoint(clickPosition);
        }

        if (_points.Contains(newPoint))
        {
            return;
        }

        _points.Add(newPoint);

        if (_points.Count == 4)
        {
            IFigure bezierCurve = _context.CreateFigure(ObjectType.CubicBezier, _points.ToArray());
            _commandsBuffer.AddCommand(new AddCommand(_context, bezierCurve));
            OnToolChanged();
            return;
        }
    }

    public void MouseUp(Vector2 clickPosition)
    {
        return;
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
        _points.Clear();
    }
}
