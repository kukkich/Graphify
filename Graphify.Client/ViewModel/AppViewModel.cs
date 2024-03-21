using System.IO;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Graphify.Client.Model;
using Graphify.Client.Model.Draw;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Client.ViewModel;

public class AppViewModel : ReactiveObject
{

    public IReadOnlyList<IReactiveCommand> AllCommands => _allCommands;

    private readonly List<IReactiveCommand> _allCommands;

    [Reactive] public int ReactiveProperty { get; private set; }
    [Reactive] public IGeometricObject? EditingObject { get; set; }
    public SourceCache<IGeometricObject, IGeometricObject> GeometryObjects { get; set; }

    public ReactiveCommand<Vector2, Unit> RightMouseUp { get; private set; }
    public ReactiveCommand<Vector2, Unit> RightMouseDown { get; private set; }

    public ReactiveCommand<Vector2, Unit> MouseDown { get; private set; }
    public ReactiveCommand<Vector2, Unit> MouseUp { get; private set; }
    public ReactiveCommand<Vector2, Unit> MouseMove { get; private set; }

    public ReactiveCommand<Unit, Unit> Redo { get; private set; }
    public ReactiveCommand<Unit, Unit> Undo { get; private set; }

    public ReactiveCommand<Unit, Unit> Copy { get; private set; }
    public ReactiveCommand<Unit, Unit> Paste { get; private set; }
    public ReactiveCommand<Unit, Unit> Cut { get; private set; }
    public ReactiveCommand<Unit, Unit> Clean { get; private set; }

    public ReactiveCommand<Unit, Unit> Delete { get; private set; }

    public ReactiveCommand<Unit, Unit> SelectAll { get; private set; }

    public ReactiveCommand<Unit, Unit> ZoomIn { get; private set; }
    public ReactiveCommand<Unit, Unit> ZoomOut { get; private set; }
    public ReactiveCommand<EditMode, Unit> SetEditMode { get; private set; }

    public ReactiveCommand<Unit, Unit> OpenExportDialogCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> OpenImportDialogCommand { get; private set; }

    public ReactiveCommand<(string Path, ExportFileType Format), Unit> Export { get; private set; }
    public ReactiveCommand<(string Path, ImportFileType Format), Unit> Import { get; private set; }

    private readonly ILogger<AppViewModel> _logger;
    private readonly Application _application;
    private IApplicationTool _currentTool;

    public AppViewModel(ILogger<AppViewModel> logger, Application application)
    {
        _logger = logger;
        _application = application;
        _currentTool = application.ToolsController.ChangeTool(EditMode.Move);


        SetEditMode = ReactiveCommand.CreateFromObservable<EditMode, Unit>(SetMode);
        Export = ReactiveCommand.CreateFromTask<(string Path, ExportFileType Format), Unit>(ExportTo);
        Import = ReactiveCommand.CreateFromTask<(string Path, ImportFileType Format), Unit>(ImportFrom);
        OpenExportDialogCommand = ReactiveCommand.CreateFromObservable(OpenExportDialog);
        OpenImportDialogCommand = ReactiveCommand.CreateFromObservable(OpenImportDialog);

        RightMouseUp = ReactiveCommand.CreateFromObservable<Vector2, Unit>(RightMouseUpAction);
        RightMouseDown = ReactiveCommand.CreateFromObservable<Vector2, Unit>(RightMouseDownAction);

        MouseDown = ReactiveCommand.CreateFromObservable<Vector2, Unit>(MouseDownAction);
        MouseUp = ReactiveCommand.CreateFromObservable<Vector2, Unit>(MouseUpAction);
        MouseMove = ReactiveCommand.CreateFromObservable<Vector2, Unit>(MouseMoveAction);

        Undo = ReactiveCommand.CreateFromObservable(UndoChanges);
        Redo = ReactiveCommand.CreateFromObservable(RedoChanges);

        Copy = ReactiveCommand.CreateFromObservable(CopyObjects);
        Cut = ReactiveCommand.CreateFromObservable(CutObjects);
        Paste = ReactiveCommand.CreateFromObservable(PasteObjects);
        Clean = ReactiveCommand.CreateFromObservable(CleanObjects);

        Delete = ReactiveCommand.CreateFromObservable(DeleteObjects);
        SelectAll = ReactiveCommand.CreateFromObservable(SelectAllObjects);

        EditingObject = null;

        GeometryObjects = new SourceCache<IGeometricObject, IGeometricObject>(a => a);
        // Test example, todo: remove if it'll work
        //GeometryObjects.AddOrUpdate([
        //    new Point(1,1),
        //    new Point(2,2),
        //    new CubicBezierCurve([new Point(1,0), new Point(0,0), new Point(0, 1), new Point(0, 4)]),
        //    new Circle( new Point(1,1), new Point(2,2)),
        //    new Line(new Point(1,1),
        //    new Point(3,2))
        //]);

        _application.Context.Surface.OnGeometryObjectAddedEvent += newObject => GeometryObjects.AddOrUpdate(newObject);
        _application.Context.Surface.OnGeometryObjectRemovedEvent += removedObject => GeometryObjects.Remove(key: removedObject);


        var type = GetType();
        var properties = type.GetProperties();

        var commandProperties = properties
            .Where(prop => typeof(IReactiveCommand).IsAssignableFrom(prop.PropertyType))
            .Select(prop => (IReactiveCommand)prop.GetValue(this))
            .Where(prop => prop is not null)
            .ToList();

        _allCommands = commandProperties;
    }

    

    public SaveFileDialog InitializeExportDialog()
    {
        SaveFileDialog exportFileDialog = new SaveFileDialog
        {
            FileName = "test",
            DefaultExt = ".svg",
            Filter = "SVG image (*.svg)|*.svg|PNG image (*.png)|*.png|Grafify image (*.grafify)|*.grafify",
            InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
            CheckFileExists = false
        };
        return exportFileDialog;
    }
    private ExportFileType SelectExportFileType(string selectedExtension)
    {
        ExportFileType fileType = selectedExtension switch
        {
            ".svg" => ExportFileType.Svg,
            ".png" => ExportFileType.Png,
            ".grafify" => ExportFileType.Custom,
            _ => throw new InvalidOperationException(selectedExtension)
        };
        return fileType;
    }
    
    private ImportFileType SelectImportFileType(string selectedExtension)
    {
        ImportFileType fileType = selectedExtension switch
        {
            ".png" => ImportFileType.Png,
            ".grafify" => ImportFileType.Custom,
            _ => throw new InvalidOperationException(selectedExtension)
        };
        
        return fileType;
    }

    public string GetFilePath(SaveFileDialog exportFileDialog)
    {
        string filePath = exportFileDialog.FileName;
        return filePath;
    }
    
    public ExportFileType GetExportFileType(string path)
    {
        string selectedExtension = Path.GetExtension(path);
        ExportFileType type = SelectExportFileType(selectedExtension);
        return type;
    }
    
    public ImportFileType GetImportFileType(string path)
    {
        string selectedExtension = Path.GetExtension(path);
        ImportFileType type = SelectImportFileType(selectedExtension);
        return type;
    }

    private IObservable<Unit> OpenExportDialog()
    {
        var exportFileDialog = InitializeExportDialog();
        if (exportFileDialog.ShowDialog() != true)
        {
            return Observable.Return(Unit.Default);
        }
        var filePath = GetFilePath(exportFileDialog);
        var fileType = GetExportFileType(filePath);
        Export.Execute((filePath, fileType));

        return Observable.Return(Unit.Default);
    }

    public string GetFilePath(OpenFileDialog importFileDialog)
    {
        string filePath = importFileDialog.FileName;
        return filePath;
    }

    private OpenFileDialog InitializeImportDialog()
    {
        OpenFileDialog importFileDialog = new OpenFileDialog
        {
            FileName = "test",
            DefaultExt = ".grafify",
            Filter = "Grafify image (*.grafify)|*.grafify",
            InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
            CheckFileExists = false
        };
        return importFileDialog;
    }
    private IObservable<Unit> OpenImportDialog()
    {
        var importFileDialog = InitializeImportDialog();
        if (importFileDialog.ShowDialog() != true)
        {
            return Observable.Return(Unit.Default);
        }
        var filePath = GetFilePath(importFileDialog);
        var fileType = GetImportFileType(filePath);
        Import.Execute((filePath, fileType));

        return Observable.Return(Unit.Default);
    }    

    private Task<Unit> ExportTo((string Path, ExportFileType Format) tuple)
    {
        _application.Exporter.Export(tuple.Format, tuple.Path);
        return Task.FromResult(Unit.Default);
    }
    private Task<Unit> ImportFrom((string Path, ImportFileType Format) tuple)
    {
        _application.Importer.Import(tuple.Format,  tuple.Path);
        return Task.FromResult(Unit.Default);
    }

    private IObservable<Unit> RightMouseDownAction(Vector2 position)
    {
        _currentTool.RightMouseDown(position);
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> RightMouseUpAction(Vector2 position)
    {
        _currentTool.RightMouseUp(position);
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> MouseDownAction(Vector2 position)
    {
        _currentTool.MouseDown(position);
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> MouseUpAction(Vector2 position)
    {
        _currentTool.MouseUp(position);
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> MouseMoveAction(Vector2 position)
    {
        _currentTool.MouseMove(position);
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> UndoChanges()
    {
        _application.CommandsBuffer.Undo();
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> RedoChanges()
    {
        _application.CommandsBuffer.Redo();
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> CopyObjects()
    {
        _application.Copy();
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> CutObjects()
    {
        _application.Cut();
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> PasteObjects()
    {
        _application.Paste();
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> SetMode(EditMode mode)
    {
        _currentTool = _application.ToolsController.ChangeTool(mode);
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> DeleteObjects()
    {
        _application.Delete();
        return Observable.Return(Unit.Default);
    }

   
    private IObservable<Unit> SelectAllObjects()
    {
        _application.Context.SelectAll();
        return Observable.Return(Unit.Default);
    }
    private IObservable<Unit> CleanObjects()
    {
        _application.Clear();
        GeometryObjects.Clear();
        return Observable.Return(Unit.Default);
    }
}
