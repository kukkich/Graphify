using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Commands;

public class PasteCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly Clipboard _clipboard;
    private IEnumerable<IGeometricObject> _pastedObjects;

    public PasteCommand(ApplicationContext context, Clipboard clipboard)
    {
        _context = context;
        _clipboard = clipboard;
        _pastedObjects = new List<IGeometricObject>();
    }

    public void Execute()
    {
        _pastedObjects = _clipboard.PasteObjects();

        foreach (var pastedObject in _pastedObjects)
        {
            _context.Surface.AddObject(pastedObject);
        }
    }

    public void Undo()
    {
        foreach (var pastedObject in _pastedObjects)
        {
            _context.Surface.TryRemove(pastedObject);
        }

        _clipboard.CopyObjects(_pastedObjects);
    }
}
