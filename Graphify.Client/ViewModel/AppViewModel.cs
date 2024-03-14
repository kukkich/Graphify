using System;
using System.Numerics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using Graphify.Core;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat.ModeDetection;

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

    public AppViewModel(ILogger<AppViewModel> logger)
    {
        _logger = logger;
        
        IncrementCommand = ReactiveCommand.CreateFromObservable(Increment);
        IncrementCommand.Subscribe(_ =>
        {
            _logger.LogDebug("Increment invoked. New value {value}", ReactiveProperty);
        });
        SetEditMode = ReactiveCommand.CreateFromObservable<EditMode, Unit>(SetMode);
        Export = ReactiveCommand.CreateFromTask<(string Path, ExportFileFormat Format), Unit>(async tuple =>
        {
            string path = tuple.Path;
            ExportFileFormat format = tuple.Format;

            // действия по экспорту файла с указанным путем и форматом
            await Task.Delay(1000); // пример асинхронной операции

            return Unit.Default;
        });
    }
    //TODO реализовать
    private IObservable<Unit> SetMode(EditMode mode)
    {
        return Observable.Return(Unit.Default);
    }
    private IObservable<Unit> Increment()
    {
        ReactiveProperty++;
        return Observable.Return(Unit.Default);
    }
}

public enum EditMode
{
    Move,
    CreatePoint,
    CreateLine,
    CreatePolygon,
    CreateCircle,
    CreateCurve
}

public enum ExportFileFormat
{

}
