using Graphify.Geometry.Drawing;

namespace Graphify.Client.View.Drawing.Base;

public interface IGeometryObjectDrawer<TDrawParams>
{
    public void Draw(TDrawParams parameters, ObjectState objectState, DrawSettings settings);
    public Dictionary<ObjectState, Action<TDrawParams, DrawSettings>> GetDrawActions();
}

