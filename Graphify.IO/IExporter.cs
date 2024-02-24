using System.Numerics;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.IO;

public interface IExporter
{
    public void ExportPoint(Point point);

    public void ExportFigure(
        IFigure figure, 
        ObjectType figureType, 
        Vector2 leftBottomBound, 
        Vector2 rightTopBound,
        IStyle style
    );

    public void SaveFile(string path);
}
