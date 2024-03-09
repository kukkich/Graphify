using System.Numerics;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class MoveCommand : ICommand
{
    private readonly IGeometricObject _geometricObject;
    private readonly Vector2 _shift;

    public MoveCommand(IGeometricObject geometricObject, Vector2 shift)
    {
        _geometricObject = geometricObject;
        _shift = shift;
    }

    public void Execute()
    {
        _geometricObject.Move(_shift);
    }

    public void Undo()
    {
        _geometricObject.Move(-_shift);
    }
}
