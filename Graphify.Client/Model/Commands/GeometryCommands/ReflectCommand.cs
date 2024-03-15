using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Commands;

public class ReflectCommand : ICommand
{
    private readonly IEnumerable<IGeometricObject> _geometricObjects;
    private readonly Point _point;

    public ReflectCommand(IEnumerable<IGeometricObject> geometricObjects, Point point)
    {
        _geometricObjects = geometricObjects;
        _point = point;
    }

    public void Execute()
    {
        foreach (var geometricObject in _geometricObjects)
        {
            geometricObject.Reflect(_point);
        }
    }

    public void Undo()
    {
        Execute();
    }
}
