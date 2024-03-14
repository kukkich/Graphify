using System.Numerics;

namespace Graphify.Client.Model.Interfaces;

// TODO change tools interface
public interface IApplicationTool
{
    void RightMouseDown(Vector2 clickPosition);
    void RightMouseUp(Vector2 clickPosition);
    public void MouseMove(Vector2 newPosition);
    public void MouseDown(Vector2 clickPosition);
    void MouseUp(Vector2 clickPosition);

    public bool InProgress();
    public void Cancel();
    public void Reset();
}
