using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Client.ViewModel;

public class AppViewModel : ReactiveObject
{
    [Reactive] public int ReactiveProperty { get; private set; }
    public ReactiveCommand<Unit, Unit> IncrementCommand { get; }

    private readonly ILogger<AppViewModel> _logger;

    public AppViewModel(ILogger<AppViewModel> logger)
    {
        _logger = logger;
        IncrementCommand = ReactiveCommand.CreateFromObservable(Increment);
        IncrementCommand.Subscribe(_ =>
        {
            _logger.LogDebug("Increment invoked. New value {value}", ReactiveProperty);
        });
    }

    private IObservable<Unit> Increment()
    {
        ReactiveProperty++;
        return Observable.Return(Unit.Default);
    }
}
