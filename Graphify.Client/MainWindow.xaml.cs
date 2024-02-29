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
using System.Windows.Input;
using Graphify.Client.Model.Enums;

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
    }

    private void GlWindow_Resized(object sender, OpenGLRoutedEventArgs args)
    {
        _gl.Viewport(0, 0, (int)GlWindow.ActualWidth, (int)GlWindow.ActualHeight);
        _gl.LoadIdentity();
        _gl.Ortho(-GlWindow.ActualWidth / 2, GlWindow.ActualWidth / 2, - GlWindow.ActualHeight / 2, GlWindow.ActualHeight / 2, -1, 1);
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
}
