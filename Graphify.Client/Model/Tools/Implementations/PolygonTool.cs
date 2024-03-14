using System.Numerics;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class PolygonTool : IApplicationTool
{
    private readonly ApplicationContext _context;
    private readonly CommandsBuffer _commandsBuffer;

    private readonly List<Point> _points = [];
    
    public PolygonTool(ApplicationContext context, CommandsBuffer commandsBuffer)
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
        IGeometricObject geometricObject =  _context.Surface.TryGetClosestObject(clickPosition);
        
        Point newPoint;
        if (geometricObject is Point point)
        {
            newPoint = point;
        }
        else
        {
            newPoint = _context.CreatePoint(clickPosition);
        }
        
        if (_points.Count >= 3 && _points[0] == newPoint)
        {
            _context.CreateFigure(ObjectType.Line, [_points[^1], _points[0] ]);
            IFigure polygon = _context.CreateFigure(ObjectType.Polygon, _points.ToArray());
            _commandsBuffer.AddCommand(new AddCommand(_context, polygon));
            OnToolChanged();
            return;
        }

        if (_points.Contains(newPoint))
        {
            return;
        }
        _points.Add(newPoint);

        if (_points.Count >= 2)
        {
            _context.CreateFigure(ObjectType.Line, [_points[^1], _points[^2] ]);
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
