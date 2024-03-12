using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Commands;

public class RotateCommand : ICommand
{
    private readonly IGeometricObject _geometricObject;
    private readonly Point _point;
    private readonly float _angle;

    public RotateCommand(IGeometricObject geometricObject, Point point, float angle)
    {
        _geometricObject = geometricObject;
        _point = point;
        _angle = angle;
    }

    public void Execute()
    {
        _geometricObject.Rotate(_point, _angle);
    }

    public void Undo()
    {
        _geometricObject.Rotate(_point, -_angle);
    }
}
