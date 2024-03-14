namespace Graphify.Geometry.Drawing;

public interface IDrawable
{
    public void Draw(IDrawer drawer);
    public ObjectState ObjectState { get; set; }
}
