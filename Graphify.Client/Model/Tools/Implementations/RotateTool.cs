using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class RotateTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;
    private readonly CommandsBuffer _commandsBuffer;

    private Vector2 _previousMousePosition;
    private Point? _point;
    private float _angle;
    private const float RotationSensitivity = 0.5f;

    public RotateTool(ApplicationContext applicationContext, CommandsBuffer commandsBuffer) 
    {
        _applicationContext = applicationContext;
        _commandsBuffer = commandsBuffer;
    }

    public void RightMouseDown(Vector2 clickPosition) { }

    public void RightMouseUp(Vector2 clickPosition) { }

    public void MouseMove(Vector2 newPosition)
    {
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            if (_point is null)
            {
                return;
            }

            _angle += (newPosition.Y - _previousMousePosition.Y) * RotationSensitivity;

            Rotate(_point, (newPosition.Y - _previousMousePosition.Y) * RotationSensitivity);
        }

        _previousMousePosition = newPosition;
    }

    private void Rotate(Point point, float angle)
    {
        foreach (var geometricObject in _applicationContext.SelectedObjects)
        {
            geometricObject.Rotate(point, angle);
        }
    }
    
    public void MouseDown(Vector2 clickPosition)
    {
        IGeometricObject closestObject = _applicationContext.Surface.TryGetClosestObject(clickPosition);

        if (Keyboard.IsKeyDown(Key.LeftCtrl))
        {
            if (_applicationContext.SelectedObjects.Contains(closestObject) && closestObject != _point)
            {
                _applicationContext.UnSelect(closestObject);
                return;
            }

            _applicationContext.Select(closestObject, false);
            return;
        }

        if (!_applicationContext.SelectedObjects.Contains(closestObject) || closestObject is not Point point)
        {
            return;
        }
        
        if (_point is not null)
        {
            _point.ObjectState = ObjectState.Selected;
        }
            
        _point = point;
        _point.ObjectState = ObjectState.ControlPoint;
    }

    public void MouseUp(Vector2 clickPosition)
    {
        if (_angle >= RotationSensitivity)
        {
            _commandsBuffer.AddCommand(new RotateCommand(_applicationContext.SelectedObjects, _point, _angle));
            _angle = 0;
        }
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
        
        _point.ObjectState = ObjectState.Selected;
        _point = null;
    }
}
