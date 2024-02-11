using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Geometry.GeometricObjects;

public class BezierCurve : IFigure
{
    public string Id { get; }
    public void Update() => throw new NotImplementedException();
    public List<IAttachable> AttachedPoints { get; set; }
    public IEnumerable<Point> ControlPoints { get; }
    public IEnumerable<IAttachable> Attached => _attachedPoints.Select(x => x.Target);

    private readonly Point[] _points;
    private readonly List<(IAttachable Target, AttachmentParameter Parameter)> _attachedPoints;

    public BezierCurve(params Point[] points)
    {
        _points = points;
        _attachedPoints = new List<(IAttachable, AttachmentParameter)>();
    }

    public bool CanApply(IOperation operation)
    {
        return operation.CanBeAppliedToCurve(this);
    }

    public void Apply(IOperation operation)
    {
        operation.ApplyToCurve(this);
    }

    public void Draw(IDrawer drawer)
    {
        drawer.DrawCurve();
    }
}
