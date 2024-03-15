using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class CopyCommand : ICommand
{
    private readonly Clipboard _clipboard;
    private readonly IEnumerable<IGeometricObject> _objects;

    public CopyCommand(Clipboard clipboard, IEnumerable<IGeometricObject> objects)
    {
        _clipboard = clipboard;
        _objects = objects;
    }

    public void Execute()
    {
        _clipboard.CopyObjects(_objects);
    }

    public void Undo()
    {
        _clipboard.Clear();
    }
}
