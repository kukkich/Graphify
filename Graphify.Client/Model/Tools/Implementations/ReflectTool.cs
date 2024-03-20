using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class ReflectTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;
    private readonly CommandsBuffer _commandsBuffer;

    private Point? _point;

    public ReflectTool(ApplicationContext applicationContext, CommandsBuffer commandsBuffer)
    {
        _applicationContext = applicationContext;
        _commandsBuffer = commandsBuffer;
    }

    public void RightMouseDown(Vector2 clickPosition) { }

    public void RightMouseUp(Vector2 clickPosition) { }

    public void MouseMove(Vector2 newPosition) { }

    public void MouseDown(Vector2 clickPosition)
    {
        IGeometricObject closestObject = _applicationContext.Surface.TryGetClosestObject(clickPosition);

        if (!Keyboard.IsKeyDown(Key.LeftCtrl))
        {
            if (closestObject is Point point)
            {
                if (_point == point)
                {
                    _point.ObjectState = ObjectState.Default;
                    _point = null;
                }
                else
                {
                    if (_point == null)
                    {
                        _point = point;
                        _point.ObjectState = ObjectState.ControlPoint;
                    }
                }
            }
        }
        else
        {
            if (closestObject != null)
            {
                if (_applicationContext.SelectedObjects.Contains(closestObject) && closestObject != _point)
                {
                    _applicationContext.UnSelect(closestObject);
                    return;
                }

                _applicationContext.Select(closestObject, false);
            }
            else
            {
                _applicationContext.ClearSelected();
            }
        }
    }

    public void MouseUp(Vector2 clickPosition)
    {
        if (_point == null || !_applicationContext.SelectedObjects.Any() || Keyboard.IsKeyDown(Key.LeftCtrl)) return;

        Reflect();
        _commandsBuffer.AddCommand(new ReflectCommand(_applicationContext.SelectedObjects, _point));
    }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel() { }

    public void OnToolChanged()
    {
        if (_point is null)
        {
            return;
        }

        _point.ObjectState = ObjectState.Default;
        _point = null;
        _applicationContext.ClearSelected();
    }

    private void Reflect()
    {
        foreach (var selectedObject in _applicationContext.SelectedObjects)
        {
            selectedObject.Reflect(_point);
        }
    }
}
