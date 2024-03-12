using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class CutCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly Clipboard _clipboard;
    private readonly IEnumerable<IGeometricObject> _geometricObjects;

    public CutCommand(ApplicationContext context, Clipboard clipboard, IEnumerable<IGeometricObject> geometricObjects)
    {
        _context = context;
        _clipboard = clipboard;
        _geometricObjects = geometricObjects;
    }


    public void Execute()
    {
        foreach (var geometricObject in _geometricObjects)
        {
            _context.Surface.TryRemove(geometricObject);
        }

        _clipboard.CopyObjects(_geometricObjects);
    }

    public void Undo()
    {
        foreach (var geometricObject in _geometricObjects)
        {
            _context.Surface.AddObject(geometricObject);
        }

        _clipboard.Clear();
    }
}
