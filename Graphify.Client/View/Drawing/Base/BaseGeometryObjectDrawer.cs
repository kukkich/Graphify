using System.Numerics;
using Graphify.Geometry.Drawing;
using SharpGL;

namespace Graphify.Client.View.Drawing.Base;

public abstract class BaseGeometryObjectDrawer<TDrawParams> : IGeometryObjectDrawer<TDrawParams>
{
    protected readonly IBaseDrawer defaultDrawer;

    private readonly Dictionary<ObjectState, Action<TDrawParams, DrawSettings>> _drawActions;

    protected BaseGeometryObjectDrawer(IBaseDrawer defaultDrawer)
    {
        this.defaultDrawer = defaultDrawer;

        _drawActions = GetDrawActions();
    }
    
    public Dictionary<ObjectState, Action<TDrawParams, DrawSettings>> GetDrawActions()
    {
        var drawActions = new Dictionary<ObjectState, Action<TDrawParams, DrawSettings>>
        {
            [ObjectState.Default] = DrawDefault,
            [ObjectState.Selected] = DrawSelected
        };

        return drawActions;
    }
    
    public void Draw(TDrawParams point, ObjectState objectState, DrawSettings settings)
    {
        _drawActions[objectState](point, settings);
    }

    protected abstract void DrawDefault(TDrawParams point, DrawSettings settings);
    protected abstract void DrawSelected(TDrawParams point, DrawSettings settings);
}
