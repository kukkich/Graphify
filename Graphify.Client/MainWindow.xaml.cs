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

namespace Graphify.Client;

public partial class MainWindow     
{
    private OpenGL _gl;
    private readonly ILogger<AppViewModel> _logger;
    [Reactive] public int Mode { get; private set; }
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

    private void ModeButton_Click(object sender, RoutedEventArgs e)
    {
        Button? clickedButton = sender as Button;
        if (clickedButton != null)
        {
            EditMode selectedMode = EditMode.Move;
            if (clickedButton.Name == "MoveModeButton")
            {
                selectedMode = EditMode.Move;
            }
            else if (clickedButton.Name == "CreatePointModeButton")
            {
                selectedMode = EditMode.CreatePoint;
            }
            else if (clickedButton.Name == "CreateLineModeButton")
            {
                selectedMode = EditMode.CreateLine;
            }
            AppViewModel? viewModel = DataContext as AppViewModel;
            if (viewModel != null)
            {
                viewModel.SetEditMode.Execute(selectedMode);
            }
        }
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        
    }
}
