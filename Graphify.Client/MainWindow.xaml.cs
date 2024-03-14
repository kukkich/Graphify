using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL.WPF;
using SharpGL;
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
    private OpenGL _gl;
    private readonly ILogger<AppViewModel> _logger;
    public MainWindow(AppViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.ReactiveProperty, view => view.ValueBox.Text)
                .DisposeWith(disposables);
        });
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
    
    private void CreatePolygonModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.SetEditMode.Execute(EditMode.CreatePolygon);
    }

    private void CreateCircleModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.SetEditMode.Execute(EditMode.CreateCircle);
    }

    private void CreateCurveModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.SetEditMode.Execute(EditMode.CreateCurve);
    }
    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        //реализовать выпадающее окно для выбора пути
        if (ViewModel != null)
        {
            ViewModel.Export.Execute();
        }
    }

    private void GlWindow_MouseDown(object sender, MouseButtonEventArgs args)
    {
        if (ViewModel != null)
        {
            ViewModel.MouseDown.Execute();
        }
    }

    
}
