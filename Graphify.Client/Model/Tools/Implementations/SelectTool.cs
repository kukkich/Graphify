using System.Numerics;
using Graphify.Client.Model.Interfaces;

namespace Graphify.Client.Model.Tools.Implementations;

public class SelectTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;

    public SelectTool(ApplicationContext applicationContext) 
    {
        _applicationContext = applicationContext;
    }

    public void MouseMove(Vector2 newPosition) => throw new NotImplementedException();

    public void MouseDown(Vector2 clickPosition)
    {
        _applicationContext.Select(clickPosition, false);
    }

    public void Cancel() => throw new NotImplementedException();

    public void Reset() => throw new NotImplementedException();
}
