using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Tools.Implementations;

public class MoveTool : IApplicationTool
{
    private const float SmallShift = 0.5f;

    private readonly ApplicationContext _applicationContext;
    private readonly CommandsBuffer _commandsBuffer;
    private Vector2 _previousMousePosition;
    private Vector2 _moveShift = Vector2.Zero;

    public MoveTool(ApplicationContext applicationContext, CommandsBuffer commandsBuffer)
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
            Move(newPosition);
        }

        _previousMousePosition = newPosition;
    }

    private void Move(Vector2 newPosition)
    {
        foreach (var geometricObject in _applicationContext.SelectedObjects)
        {
            var shift = (newPosition - _previousMousePosition);

            if (!(shift.Length() >= SmallShift)) continue;

            geometricObject.Move((newPosition - _previousMousePosition));
            _moveShift += newPosition - _previousMousePosition;
        }
    }

    public void MouseDown(Vector2 clickPosition)
    {
        var closestObject = _applicationContext.Surface.TryGetClosestObject(clickPosition);

        if (closestObject is null)
        {
            _applicationContext.ClearSelected();
        }

        if (Keyboard.IsKeyDown(Key.LeftCtrl))
        {
            _applicationContext.ToggleSelection(closestObject!);
        }
        else
        {
            if (!_applicationContext.SelectedObjects.Contains(closestObject))
            {
                _applicationContext.Select(closestObject!, true);
            }
        }
    }

    public void MouseUp(Vector2 clickPosition)
    {
        if (_moveShift.Length() >= SmallShift)
        {
            _commandsBuffer.AddCommand(new MoveCommand(_applicationContext.SelectedObjects, _moveShift));
            _moveShift = Vector2.Zero;
        }
    }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel() { }

    public void OnToolChanged()
    {
        _moveShift = Vector2.Zero;
    }
}
