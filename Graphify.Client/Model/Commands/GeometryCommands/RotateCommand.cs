using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Commands;

public class RotateCommand : ICommand
{
    private readonly IEnumerable<IGeometricObject> _geometricObjects;
    private readonly Point _point;
    private readonly float _angle;

    public RotateCommand(IEnumerable<IGeometricObject> geometricObjects, Point point, float angle)
    {
        _geometricObjects = geometricObjects;
        _point = point;
        _angle = angle;
    }

    public void Execute()
    {
        foreach (var geometricObject in _geometricObjects)
        {
            geometricObject.Rotate(_point, _angle);
        }
    }

    public void Undo()
    {
        foreach (var geometricObject in _geometricObjects)
        {
            geometricObject.Rotate(_point, -_angle);
        }
    }
}
