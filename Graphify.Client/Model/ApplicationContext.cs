using Graphify.Client.Model.Geometry;

namespace Graphify.Client.Model;

public class ApplicationContext
{
    public Surface Surface { get; private set; }
    public delegate void OnSurfaceChanged(Surface newSurface);
    public event OnSurfaceChanged OnSurfaceChangedEvent;
    
    
    public ApplicationContext(Surface surface)
    {
        Surface = surface;
        OnSurfaceChangedEvent?.Invoke(surface);
    }

    public void SetSurface(Surface newSurface)
    {
        Surface = newSurface;
        OnSurfaceChangedEvent?.Invoke(newSurface);
    }
}
