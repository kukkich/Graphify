using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Interfaces;

namespace Graphify.Client.Model.Tools.Implementations;

public class SelectTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;

    public SelectTool(ApplicationContext applicationContext) 
    {
        _applicationContext = applicationContext;
    }

    public void RightMouseDown(Vector2 clickPosition) { }

    public void RightMouseUp(Vector2 clickPosition) { }

    public void MouseMove(Vector2 newPosition)
    {

    }

    public void MouseDown(Vector2 clickPosition)
    {
        _applicationContext.Select(clickPosition, false);
    }

    public void MouseUp(Vector2 clickPosition) { }

    public bool InProgress()
    {
        return Keyboard.IsKeyDown(Key.LeftCtrl);
    }

    public void Cancel() { }

    public void Reset() { }
}
