using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Graphify.Client.Model;
using Graphify.Client.Model.Enums;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Client.ViewModel;

public class AppViewModel : ReactiveObject
{
    [Reactive] public int ReactiveProperty { get; private set; }
    [Reactive] public IGeometricObject? EditingObject { get; set; }
    public SourceList<IGeometricObject> GeometryObjects { get; set; }

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
    
    public ReactiveCommand<Unit, Unit> SelectAll { get; private set; }
    
    public ReactiveCommand<Unit, Unit> ZoomIn { get; private set; }
    public ReactiveCommand<Unit, Unit> ZoomOut { get; private set; }
    public ReactiveCommand<EditMode, Unit> SetEditMode { get; private set; }
    
    public ReactiveCommand<(string Path, ExportFileType Format), Unit> Export { get; private set; }
    public ReactiveCommand<string, Unit> Import { get; private set; }

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

        SelectAll = ReactiveCommand.CreateFromObservable(SelectAllObject);
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

    private Task<Unit> ExportTo((string Path, ExportFileType Format) tuple)
    {
        _application.Exporter.Export(tuple.Format, tuple.Path);
        return Task.FromResult(Unit.Default);
    }
    
    private IObservable<Unit> SelectAllObject()
    {
        _application.Context.SelectAll();
        return Observable.Return(Unit.Default);
    }
}
