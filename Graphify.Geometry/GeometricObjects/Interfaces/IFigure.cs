using Graphify.Geometry.Attaching;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IFigure : IAttachmentConsumer, IDrawable, IInteractive
{
    
}

public interface IDrawable
{
    public void Draw(IDrawer drawer);
}

public interface IDrawer
{
    public void DrawCurve();
    public void DrawLine();
    public void DrawPoint();
}
