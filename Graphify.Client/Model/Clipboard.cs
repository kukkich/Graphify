using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model;

public class Clipboard
{
    private readonly LinkedList<IGeometricObject> _copiedObjects;

    public Clipboard() 
    {
        _copiedObjects = new LinkedList<IGeometricObject>();
    }

    public void CopyObject(IGeometricObject geometricObject)
    {
        Clear();

        _copiedObjects.AddLast(geometricObject);
    }

    public void CopyObjects(IEnumerable<IGeometricObject> geometricObjects)
    {
        Clear();

        foreach (var geometricObject in geometricObjects)
        {
            _copiedObjects.AddLast(geometricObject);
        }
    }

    public IEnumerable<IGeometricObject> PasteObjects()
    {
        var pastedObjects = _copiedObjects.Select(c => c.Clone());

        return pastedObjects;
    }

    // TODO remove if unused
    public void RemoveObjects(IEnumerable<IGeometricObject> geometricObjects)
    {
        foreach (var _ in geometricObjects)
        {
            _copiedObjects.RemoveLast();
        }
    }

    public void Clear()
    {
        _copiedObjects.Clear();
    }
}
