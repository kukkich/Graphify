using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL.WPF;
using SharpGL;
using Graphify.Client.View.Drawing;
using System.Windows;
using System.Reactive.Linq;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using ReactiveUI.Fody.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Windows.Input;

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
                .Where(glWindow => glWindow is not null)
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
        /*
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
        */


        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.ReactiveProperty, view => view.ValueBox.Text)
                .DisposeWith(disposables);
        });
    }

    private void MoveModeButton_Click(object sender, RoutedEventArgs e)
    {
        Button? clickedButton = sender as Button;
        if (clickedButton is not null)
        {
            EditMode selectedMode = EditMode.Move;
            if (ViewModel is not null)
            {
                ViewModel.SetEditMode.Execute(selectedMode);
            }
        }
    }

    private void CreatePointModeButton_Click(object sender, RoutedEventArgs e)
    {
        Button? clickedButton = sender as Button;
        if (clickedButton is not null)
        {
            EditMode selectedMode = EditMode.CreatePoint;
            if (ViewModel is not null)
            {
                ViewModel.SetEditMode.Execute(selectedMode);
            }
        }
    }

    private void CreateLineModeButton_Click(object sender, RoutedEventArgs e)
    {
        Button? clickedButton = sender as Button;
        if (clickedButton is not null)
        {
            EditMode selectedMode = EditMode.CreateLine;
            if (ViewModel is not null)
            {
                ViewModel.SetEditMode.Execute(selectedMode);
            }
        }
    }
    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        //реализовать выпадающее окно для выбора пути
        if (ViewModel is not null)
        {
            ViewModel.Export.Execute();
        }
    }

    private void GlWindow_MouseDown(object sender, MouseButtonEventArgs args)
    {
        if (ViewModel is not null)
        {
            ViewModel.MouseDown.Execute();
        }
    }
}
