using System.Numerics;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class PointTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;
    private readonly CommandsBuffer _commandsBuffer;

    public PointTool(ApplicationContext applicationContext, CommandsBuffer commandsBuffer)
    {
        _applicationContext = applicationContext;
        _commandsBuffer = commandsBuffer;
    }

    public void RightMouseDown(Vector2 clickPosition)
    {
        MouseDown(clickPosition);
    }

    public void RightMouseUp(Vector2 clickPosition) { }

    public void MouseMove(Vector2 newPosition)
    {

    }

    public void MouseDown(Vector2 clickPosition)
    {
        Point newPoint = _applicationContext.AddPoint(clickPosition);
        _commandsBuffer.AddCommand(new AddCommand(_applicationContext, newPoint));
    }

    public void MouseUp(Vector2 clickPosition){ }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel(){ }

    public void OnToolChanged(){ }
}
