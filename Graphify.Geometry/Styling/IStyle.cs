using System.Drawing;

namespace Graphify.Geometry.Styling;

public interface IStyle
{
    public Color PrimaryColor { get; set; }
    public string Name { get; set; }
}
