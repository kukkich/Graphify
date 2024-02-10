using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;

namespace Graphify.Client;

public partial class MainWindow
{
    public MainWindow(AppViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.ReactiveProperty, view => view.ValueBox.Text)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.IncrementCommand, view => view.IncrementButton)
                .DisposeWith(disposables);
        });
    }
}
