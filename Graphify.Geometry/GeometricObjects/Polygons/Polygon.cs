using System.Numerics;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Polygons;

public class Polygon : ReactiveObject, IFigure, IStyled<PolygonStyle>
{
    public IEnumerable<Point> Attached { get; }
    public IEnumerable<Point> ControlPoints { get; }
    [Reactive] public PolygonStyle Style { get; set; }

    public Polygon()
    {
        throw new NotImplementedException(); //TODO: реализация логики полигона
    }

    public void Update() => throw new NotImplementedException();
    public bool IsNextTo(Vector2 point, float distance) => throw new NotImplementedException();
    public void Move(Vector2 shift) => throw new NotImplementedException();
    public void ConsumeAttach(Point attachable) => throw new NotImplementedException();
    public void ConsumeDetach(Point attachable) => throw new NotImplementedException();
    public void Rotate(Point shift, float angle) => throw new NotImplementedException();
    public void Reflect(Point point) => throw new NotImplementedException();
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
    public FigureExportData GetExportData() => throw new NotImplementedException();
}
