using System.Numerics;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Geometry.GeometricObjects.Points;

public class Point : IGeometricObject, IAttachable, IStyled<PointStyle>
{
    public float X { get; }
    public float Y { get; }
    public IFigure? AttachedTo { get; }
    public IEnumerable<IFigure> ControlFor { get; }
    public PointStyle Style { get; set; }

    public Point(float x, float y)
    {
        X = x;
        Y = y;
        ControlFor = new List<IFigure>();
    }

    public bool IsNextTo(Vector2 point, float distance) => throw new NotImplementedException();
    public void Move(Vector2 shift) => throw new NotImplementedException();
    public void Rotate(System.Drawing.Point shift, float angle) => throw new NotImplementedException();
    public void Reflect(System.Drawing.Point point) => throw new NotImplementedException();
    public void Update() => throw new NotImplementedException();
    public bool CanAttachTo(IFigure consumer) => throw new NotImplementedException();
    public void AttachTo(IFigure consumer) => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
}
