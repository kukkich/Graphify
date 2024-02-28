using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using Graphify.Client.Model;
using Graphify.Core;
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
    
    public ReactiveCommand<Unit, Unit> IncrementCommand { get; private set; }
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
    public ReactiveCommand<(string Path, ExportFileFormat Format), Unit> Export { get; private set; }
    public ReactiveCommand<string, Unit> Import { get; private set; }

    private readonly ILogger<AppViewModel> _logger;
    private readonly Application _application;

    public AppViewModel(ILogger<AppViewModel> logger, Application application)
    {
        _logger = logger;
        _application = application;
        
        IncrementCommand = ReactiveCommand.CreateFromObservable(Increment);
        IncrementCommand.Subscribe(_ =>
        {
            _logger.LogDebug("Increment invoked. New value {value}", ReactiveProperty);
        });

        _application.AddPoint(new Point(1f, 1f));
        _application.UndoAction();
        _application.RedoAction();
    }

    private IObservable<Unit> Increment()
    {
        ReactiveProperty++;
        return Observable.Return(Unit.Default);
    }
}

public enum EditMode { }

public enum ExportFileFormat
{

}
