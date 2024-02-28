using Graphify.Geometry.GeometricObjects.Interfaces;
using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Geometry;

public class Surface : IGeometryContext
{
    public IEnumerable<IGeometricObject> Objects => _objects;
    public IEnumerable<IFigure> Figures => _figures;
    public IEnumerable<Point> Points => _points;

    private HashSet<IGeometricObject> _objects = [];
    private HashSet<IFigure> _figures = [];
    private HashSet<Point> _points = [];

    public IGeometricObject? TryGetClosestObject(Vector2 point, double precision) => throw new NotImplementedException();

    public Point? TryGetClosestPoint(Vector2 point, double precision) => throw new NotImplementedException();

    public IFigure? TryGetClosestFigure(Vector2 point, double precision) => throw new NotImplementedException();

    private void AddObject(IGeometricObject newObject)
    {
        _objects.Add(newObject);
    }

    public void AddPoint(Point newPoint)
    {
        _points.Add(newPoint);
        AddObject(newPoint);
    }

    public void AddFigure(IFigure newFigure)
    {
        _figures.Add(newFigure);
        AddObject(newFigure);
    }

    public bool TryRemove(IGeometricObject target)
    {
        if (_objects.Remove(target))
        {
            if (_points.Remove((Point)target))
            {
                return true;
            }
            else if(_figures.Remove((IFigure)target))
            {
                return true;
            }

            throw new ArgumentOutOfRangeException("REROU");
        }
        return false;
    }

    public void Clear()
    {
        _objects.Clear();
        _figures.Clear();
        _points.Clear();
    }
}
