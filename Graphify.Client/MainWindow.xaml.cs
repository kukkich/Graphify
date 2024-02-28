using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL.WPF;
using SharpGL;
using Graphify.Client.View.Drawing;
using System.Windows;
using System.Reactive.Linq;
using System.Numerics;

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
        _gl.Viewport(0, 0, (int)GlWindow.ActualWidth, (int)GlWindow.ActualHeight);
        _gl.LoadIdentity();
        _gl.Ortho(-GlWindow.ActualWidth / 2, GlWindow.ActualWidth / 2, - GlWindow.ActualHeight / 2, GlWindow.ActualHeight / 2, -1, 1);
    }

    private void GlWindow_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        _drawer.DrawPoint(new Vector2(0.7f,0.7f));

        List<Vector2> Polygon = new List<Vector2>
        {
            new Vector2(0,0),
            new Vector2(0.7f, 0.7f),
            new Vector2(0.7f, 0),
        };

        _drawer.DrawPolygon(Polygon);

        List<Vector2> Line = new List<Vector2>
        {
            new Vector2(-1,0),   
            new Vector2(0, -0.5f),
        };

        _drawer.DrawPolygon(Line);

        _drawer.DrawCircle(new Vector2(0, 100), 300);

        List<Vector2> Curve = new List<Vector2>
        {
            new Vector2(50,200),
            new Vector2(200,120),
            new Vector2(55,284),
            new Vector2(200,200),
        };

        _drawer.DrawBezierCurve(Curve);

    }
}
