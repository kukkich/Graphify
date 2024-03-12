using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Commands;

public class ReflectCommand : ICommand
{
    private readonly IGeometricObject _geometricObject;
    private readonly Point _point;

    public ReflectCommand(IGeometricObject geometricObject, Point point)
    {
        _geometricObject = geometricObject;
        _point = point;
    }

    public void Execute()
    {
        _geometricObject.Reflect(_point);
    }

    public void Undo()
    {
        Execute();
    }
}
