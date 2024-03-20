using System.Numerics;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IGeometricObject : IDrawable
{
    public bool IsNextTo(Vector2 point, float distance);
    public void Move(Vector2 shift);
    public void Rotate(Point shift, float angle);
    public void Reflect(Point point);
    public IGeometricObject Clone();
}
