using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Tools.Implementations;

public class MoveTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;
    private Vector2 _previousMousePosition;

    public MoveTool(ApplicationContext applicationContext) 
    {
        _applicationContext = applicationContext;
    }

    public void RightMouseDown(Vector2 clickPosition) { }

    public void RightMouseUp(Vector2 clickPosition) { }

    public void MouseMove(Vector2 newPosition)
    {
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            foreach (var geometricObject in _applicationContext.SelectedObjects)
            {
                geometricObject.Move((newPosition - _previousMousePosition));
            }
        }

        _previousMousePosition = newPosition;
    }

    private void Move(Vector2 newPosition)
    {
        foreach (var geometricObject in _applicationContext.SelectedObjects)
        {
            geometricObject.Move(newPosition - _previousMousePosition);   
        }
    }
    
    public void MouseDown(Vector2 clickPosition)
    {
        IGeometricObject closestObject = _applicationContext.Surface.TryGetClosestObject(clickPosition);

        if (closestObject is null)
        {
            _applicationContext.ClearSelected();
        }

        if (_applicationContext.SelectedObjects.Contains(closestObject))
        {
            return;
        }
        else
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _applicationContext.Select(clickPosition, false);
            }
            else
            {
                _applicationContext.Select(clickPosition, true);
            }
        }
    }

    public void MouseUp(Vector2 clickPosition) { }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel() { }
    public void Reset() { }
}
