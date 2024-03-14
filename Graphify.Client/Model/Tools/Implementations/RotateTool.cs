using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class RotateTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;

    private Vector2 _previousMousePosition;
    private Point? _point;
    private const float RotationSensitivity = 0.5f;

    public RotateTool(ApplicationContext applicationContext) 
    {
        _applicationContext = applicationContext;
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

    public void MouseUp(Vector2 clickPosition) { }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel() { }

    public void Reset()
    {
        if (_point is null)
        {
            return;
        }
        
        _point.ObjectState = ObjectState.Selected;
        _point = null;
    }
}
