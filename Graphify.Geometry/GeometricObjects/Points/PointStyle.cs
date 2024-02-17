using Graphify.Geometry.GeometricObjects.Curves;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Points;

public class PointStyle : CurveStyle
{
    [Reactive] public PointVariant Variant { get; set; }
}
