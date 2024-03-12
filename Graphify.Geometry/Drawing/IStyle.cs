using System.Drawing;

namespace Graphify.Geometry.Drawing;

public interface IStyle
{
    public Color PrimaryColor { get; set; }
    public string Name { get; set; }
    public void ApplyStyle(IDrawer drawer);
    public ObjectState ObjectState { get; set; }
}
