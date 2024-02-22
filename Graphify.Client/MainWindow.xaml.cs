using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL.WPF;
using SharpGL;
using Graphify.Client.View.Drawing;
using System.Windows;
using System.Reactive.Linq;

namespace Graphify.Client;

public partial class MainWindow     
{
    private readonly OpenGLDrawer _drawer;
    private OpenGL _gl;
    
    public MainWindow(AppViewModel viewModel, OpenGLDrawer drawer)
    {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
        _drawer = drawer;

        this.WhenActivated(disposables =>
        {
            this.WhenAnyValue(x => x.GlWindow)
                .Where(glWindow => glWindow != null)
                .Subscribe(glWindow =>
                {
                    _gl = glWindow.OpenGL;
                    _drawer.InitGl(_gl);
                })
                .DisposeWith(disposables);


        });
    }

    private void GlWindow_Resized(object sender, OpenGLRoutedEventArgs args)
    {

    }
}
