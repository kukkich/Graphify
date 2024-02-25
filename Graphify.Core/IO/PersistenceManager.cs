using System.Reflection;
using Graphify.Core.Geometry;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.IO;
using Point = Graphify.Geometry.GeometricObjects.Points.Point;

namespace Graphify.Core.IO;

public class PersistenceManager
{
    private readonly ApplicationContext _applicationContext;
    
    private readonly Dictionary<string, IExporter> _exporters = [];
    private readonly Dictionary<string, IImporter> _importers = [];

    private IExporter _selectedExporter;
    private IImporter _selectedImporter;

    public PersistenceManager(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
        
        CreateExporters();
        CreateImporters();
    }
    
    private void CreateExporters()
    {
        IEnumerable<IExporter> exporters = GetInstancesOfInterface<IExporter>();

        foreach (var exporter in exporters)
        {
            _exporters.Add(exporter.ToString(), exporter);
        }
    }
    
    private void CreateImporters()
    {
        IEnumerable<IImporter> importers = GetInstancesOfInterface<IImporter>();

        foreach (var importer in importers)
        {
            _importers.Add(importer.ToString(), importer);
        }
    }

    private IEnumerable<T> GetInstancesOfInterface<T>()
    {
        var assembly = Assembly.GetAssembly(typeof(T));

        return assembly.GetTypes()
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(type => (T)Activator.CreateInstance(type))
            .ToList();
    }

    public void SelectExporter(string id)
    {
        if (!_exporters.TryGetValue(id, out IExporter? exporter))
        {
            throw new ArgumentException("Unknown exporter id");
        }
        
        _selectedExporter = exporter;
    }

    public void SelectImporter(string id)
    {
        if (!_importers.TryGetValue(id, out IImporter? importer))
        {
            throw new ArgumentException("Unknown importer id");
        }

        _selectedImporter = importer;
    }

    // TODO need to be changed
    public void Import(string path)
    {
        if (_selectedExporter is null)
        {
            throw new NullReferenceException("exporter is not selected");
        }
        
        foreach (Point point in _applicationContext.GeometryContext.Points)
        {
            _selectedExporter.ExportPoint(point);
        }
            
        foreach (IFigure figure in _applicationContext.GeometryContext.Figures)
        {
            //_selectedExporter.ExportFigure(figure);
        }
    }

    // TODO need to be changed
    public void Export(string path)
    {
        if (_selectedImporter is null)
        {
            throw new NullReferenceException("importer is not selected");
        }
    }
}
