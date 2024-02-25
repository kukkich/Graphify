using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL.WPF;
using SharpGL;

namespace Graphify.Client;

public partial class MainWindow     
{
    private OpenGL _gl;
    public MainWindow(AppViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.MoveModeCommand, view => view.MoveModeButton)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.CreatePointModeCommand, view => view.CreatePointModeButton)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.CreateLineModeCommand, view => view.CreateLineModeButton)
                .DisposeWith(disposables);

            this.Bind(ViewModel, vm => vm.ReactiveProperty, view => view.ValueBox.Text)
                .DisposeWith(disposables);
        });
    }

}
