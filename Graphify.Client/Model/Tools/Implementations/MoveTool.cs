using System.Numerics;
using Graphify.Client.Model.Interfaces;

namespace Graphify.Client.Model.Tools.Implementations;

public class MoveTool : IApplicationTool
{
    public void MouseMove(Vector2 newPosition) => throw new NotImplementedException();
    public void MouseDown(Vector2 clickPosition) => throw new NotImplementedException();

    public void Cancel() => throw new NotImplementedException();
    public void Reset(){}
}
