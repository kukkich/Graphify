using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO;

public class ImportResult
{
    public IEnumerable<IFigure> Figures { get; internal init; } = null!;
    public IEnumerable<Point> Points { get; internal init; } = null!;
}
