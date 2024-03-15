using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Commands;

public class AttachCommand : ICommand
{
    private readonly IFigure _figure;
    private readonly Point _point;

    public AttachCommand(IFigure figure, Point point)
    {
        _figure = figure;
        _point = point;
    }

    public void Execute()
    {
        _figure.ConsumeAttach(_point);
    }

    public void Undo()
    {
        _figure.ConsumeDetach(_point);
    }
}
