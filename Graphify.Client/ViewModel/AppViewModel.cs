using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using Graphify.Client.Model;
using Graphify.Client.Model.Enums;
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

    public ReactiveCommand<Unit, Unit> RightMouseUp { get; private set; }
    public ReactiveCommand<Unit, Unit> RightMouseDown { get; private set; }
    public ReactiveCommand<Vector2, Unit> MouseDown { get; private set; }
    public ReactiveCommand<Vector2, Unit> MouseUp { get; private set; }
    public ReactiveCommand<Vector2, Unit> MouserMove { get; private set; }
    public ReactiveCommand<Unit, Unit> ZoomIn { get; private set; }
    public ReactiveCommand<Unit, Unit> ZoomOut { get; private set; }
    public ReactiveCommand<Unit, Unit> Copy { get; private set; }
    public ReactiveCommand<Unit, Unit> Cut { get; private set; }
    public ReactiveCommand<Unit, Unit> Paste { get; private set; }
    public ReactiveCommand<EditMode, Unit> SetEditMode { get; private set; }
    public ReactiveCommand<(string Path, ExportFileType Format), Unit> Export { get; private set; }
    public ReactiveCommand<string, Unit> Import { get; private set; }

    private readonly ILogger<AppViewModel> _logger;
    private readonly Application _application;

    public AppViewModel(ILogger<AppViewModel> logger, Application application)
    {
        _logger = logger;
        _application = application;

        SetEditMode = ReactiveCommand.CreateFromObservable<EditMode, Unit>(SetMode);
        Export = ReactiveCommand.CreateFromTask<(string Path, ExportFileType Format), Unit>(ExportTo);
        MouseDown = ReactiveCommand.CreateFromObservable<Vector2, Unit>(MouseDownAction);

        Copy = ReactiveCommand.CreateFromObservable<Unit, Unit>(CopyObjects);
        Cut = ReactiveCommand.CreateFromObservable<Unit, Unit>(CutObjects);
        Paste = ReactiveCommand.CreateFromObservable<Unit, Unit>(PasteObjects);
    }

    //TODO �����������???????
    private IObservable<Unit> SetMode(EditMode mode)
    {
        _application.ToolsController.SetTool(mode);
        return Observable.Return(Unit.Default);
    }

    private Task<Unit> ExportTo((string Path, ExportFileType Format) tuple)
    {
        _application.Exporter.Export(tuple.Format, tuple.Path);
        return Task.FromResult(Unit.Default);
    }

    //TODO Implement for other figures
    private IObservable<Unit> MouseDownAction(Vector2 position)
    {
        _application.ToolsController.MouseDown(position);
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> CopyObjects(Unit _)
    {
        _application.Copy();
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> CutObjects(Unit _)
    {
        _application.Cut();
        return Observable.Return(Unit.Default);
    }

    private IObservable<Unit> PasteObjects(Unit _)
    {
        _application.Paste();
        return Observable.Return(Unit.Default);
    }
}
