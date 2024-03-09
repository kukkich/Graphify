using System.Numerics;

namespace Graphify.Client.Model.Interfaces;

public interface IApplicationTool
{
    public void MouseMove(Vector2 newPosition);
    public void MouseDown(Vector2 clickPosition);
    public void Cancel();
    public void Reset();
}
