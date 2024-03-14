using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Geometry.Drawing;

namespace Graphify.Client.View.Drawing.Point;

public abstract class BasePointDrawer  : BaseGeometryObjectDrawer<Vector2>
{
    protected BasePointDrawer(IBaseDrawer defaultDrawer) : base(defaultDrawer) { }

    public override Dictionary<ObjectState, Action<Vector2, DrawSettings>> GetDrawActions()
    {
        Dictionary<ObjectState, Action<Vector2, DrawSettings>> drawActions = base.GetDrawActions();
        drawActions.Add(ObjectState.ControlPoint, DrawControlPoint);

        return drawActions;
    }

    protected abstract void DrawControlPoint(Vector2 point, DrawSettings settings);
}
