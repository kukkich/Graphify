using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Graphify.Client.Model.Enums;
using Graphify.Client.View.Drawing;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL;
using System.Windows.Controls;
using System.Windows;
using SharpGL.WPF;

namespace Graphify.Client;

public partial class MainWindow
{
    private readonly OpenGLDrawer _drawer;
    private OpenGL _gl;

    public MainWindow(AppViewModel viewModel, OpenGLDrawer drawer)
    {
        _drawer = drawer;
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();

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

        List<Figure> listObjects = new List<Figure>() //temporarily
        {
            new Figure()
            {
                Data = "A = (-1.2, 1.33)",
                Type = "LightBlue"
            },
            new Figure()
            {
                Data = "B = (3.45, 2.1)",
                Type = "LightBlue"
            },
            new Figure()
            {
                Data = "C = (0, 0)",
                Type = "LightBlue"
            },
            new Figure()
            {
                Data = "f: Прямая(B, C)",
                Type = "LightGray"
            }
        };
        this.listGeometryObjects.ItemsSource = listObjects; //temporarily
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
        cm.PlacementTarget = sender as Button;
        cm.IsOpen = true;
    }
    private void Delete_Click(object sender, RoutedEventArgs e)
    { }
    private void Clone_Click(object sender, RoutedEventArgs e)
    { }
  
    private void GlWindow_Resized(object sender, OpenGLRoutedEventArgs args)
    {
        _gl.Viewport(0, 0, (int)GlWindow.ActualWidth, (int)GlWindow.ActualHeight);
        _gl.LoadIdentity();
        _gl.Ortho(-GlWindow.ActualWidth / 2, GlWindow.ActualWidth / 2, -GlWindow.ActualHeight / 2, GlWindow.ActualHeight / 2, -1, 1);
    }

    private void GlWindow_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
    }

    private void MoveModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }

        ViewModel?.SetEditMode.Execute(EditMode.Move);
    }

    private void CreatePointModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }

        ViewModel?.SetEditMode.Execute(EditMode.CreatePoint);
    }

    private void CreateLineModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.SetEditMode.Execute(EditMode.CreateLine);
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.Export.Execute(("", ExportFileType.Svg));
    }

    private void GlWindow_MouseDown(object sender, MouseButtonEventArgs args)
    {
        if (ViewModel is null)
        {
            return;
        }
    
        var position = args.GetPosition((OpenGLControl)sender);
        position.X -= GlWindow.ActualWidth / 2;
        position.Y = GlWindow.ActualHeight / 2 - position.Y;
        ViewModel.MouseDown.Execute(new Vector2((float)position.X, (float)position.Y));
    }
  
    public class Figure //temporarily
    {
        public string Data { get; set; }
        public string Type { get; set; }
    }
}
