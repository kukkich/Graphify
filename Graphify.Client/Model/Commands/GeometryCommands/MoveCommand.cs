using System.Numerics;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class MoveCommand : ICommand
{
    private readonly IEnumerable<IGeometricObject> _movedObjects;
    private readonly Vector2 _shift;

    public MoveCommand(IEnumerable<IGeometricObject> movedObjects, Vector2 shift)
    {
        _movedObjects = movedObjects;
        _shift = shift;
    }

    public void Execute()
    {
        foreach (var movedObject in _movedObjects)
        {
            movedObject.Move(_shift);
        }
    }

    public void Undo()
    {
        foreach (var movedObject in _movedObjects)
        {
            movedObject.Move(-_shift);
        }
    }
}
