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
using Graphify.Client.Model.Enums;
using Graphify.Geometry.Drawing;

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
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.ReactiveProperty, view => view.ValueBox.Text)
                .DisposeWith(disposables);
        });
    }

    private void MoveModeButton_Click(object sender, RoutedEventArgs e)
    {
        Button? clickedButton = sender as Button;
        if (clickedButton != null)
        {
            EditMode selectedMode = EditMode.Move;
            if (ViewModel != null)
            {
                ViewModel.SetEditMode.Execute(selectedMode);
            }
        }
    }

    private void CreatePointModeButton_Click(object sender, RoutedEventArgs e)
    {
        Button? clickedButton = sender as Button;
        if (clickedButton != null)
        {
            EditMode selectedMode = EditMode.CreatePoint;
            if (ViewModel != null)
            {
                ViewModel.SetEditMode.Execute(selectedMode);
            }
        }
    }

    private void CreateLineModeButton_Click(object sender, RoutedEventArgs e)
    {
        Button? clickedButton = sender as Button;
        if (clickedButton != null)
        {
            EditMode selectedMode = EditMode.CreateLine;
            if (ViewModel != null)
            {
                ViewModel.SetEditMode.Execute(selectedMode);
            }
        }
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        //реализовать выпадающее окно для выбора пути
        if (ViewModel != null)
        {
            ViewModel.Export.Execute(("", ExportFileType.Svg));
        }
    }

    private void GlWindow_MouseDown(object sender, MouseButtonEventArgs args)
    {
        if (ViewModel != null)
        {
            var position = args.GetPosition((OpenGLControl)sender);
            position.X -= GlWindow.ActualWidth / 2;
            position.Y = GlWindow.ActualHeight / 2 - position.Y;
            ViewModel.MouseDown.Execute(new Vector2((float)position.X, (float)position.Y));
        }
    }
}
