using Graphify.Geometry.Attaching;
using Graphify.Geometry.Export;

namespace Graphify.Geometry.GeometricObjects.Interfaces;

public interface IFigure : IAttachmentConsumer, IGeometricObject
{
    public FigureExportData GetExportData();
}
