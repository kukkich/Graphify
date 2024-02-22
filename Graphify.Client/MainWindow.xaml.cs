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
        });
    }

}
