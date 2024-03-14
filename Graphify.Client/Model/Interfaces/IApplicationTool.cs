using System.Numerics;

namespace Graphify.Client.Model.Interfaces;

// TODO change tools interface
public interface IApplicationTool
{
    public void RightMouseDown(Vector2 clickPosition);
    public void RightMouseUp(Vector2 clickPosition);
    public void MouseMove(Vector2 newPosition);
    public void MouseDown(Vector2 clickPosition);
    public void MouseUp(Vector2 clickPosition);

    public bool InProgress();
    public void Cancel();
    public void OnToolChanged();
}
