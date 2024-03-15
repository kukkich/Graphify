using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Graphify.Client.Model;
using Graphify.Client.Model.Enums;
using Graphify.Core.Model.IO.Export;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
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
    public ReactiveCommand<Unit, Unit> Redo { get; private set; }
    public ReactiveCommand<Unit, Unit> Undo { get; private set; }
    public ReactiveCommand<Unit, Unit> ZoomIn { get; private set; }
    public ReactiveCommand<Unit, Unit> ZoomOut { get; private set; }
    public ReactiveCommand<EditMode, Unit> SetEditMode { get; private set; }
    public ReactiveCommand<(string Path, ExportFileType Format), Unit> Export { get; private set; }
    public ReactiveCommand<string, Unit> Import { get; private set; }

    private readonly ILogger<AppViewModel> _logger;
    private readonly Application _application;
    // TODO remove
    private readonly Exporter _exporter;

    public AppViewModel(ILogger<AppViewModel> logger, Application application, Exporter exporter)
    {
        _logger = logger;
        _application = application;
        _exporter = exporter;

        // for tests
        GeometryObjects = new SourceList<IGeometricObject>();
        GeometryObjects.AddRange([
            new Point(1,1),
            new Point(2,2),
            new Circle( new Point(1,1), new Point(2,2))
        ]);

        SetEditMode = ReactiveCommand.CreateFromObservable<EditMode, Unit>(SetMode);
        Export = ReactiveCommand.CreateFromTask<(string Path, ExportFileType Format), Unit>(ExportTo);
        MouseDown = ReactiveCommand.CreateFromObservable<Vector2, Unit>(MouseDownAction);

        _application.AddPoint(new Vector2(1f, 1f));
        _application.UndoAction();
        _application.RedoAction();
    }

    //TODO �����������???????
    private IObservable<Unit> SetMode(EditMode mode)
    {
        return Observable.Return(Unit.Default);
    }

    private Task<Unit> ExportTo((string Path, ExportFileType Format) tuple)
    {
        _exporter.Export(tuple.Format, tuple.Path);
        return Task.FromResult(Unit.Default);
    }

    private IObservable<Unit> Increment()
    {
        ReactiveProperty++;
        return Observable.Return(Unit.Default);
    }

    //TODO Implement for other figures
    private IObservable<Unit> MouseDownAction(Vector2 position)
    {
        _application.AddPoint(position);
        return Observable.Return(Unit.Default);
    }
}
