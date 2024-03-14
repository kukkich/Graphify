using System.IO;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Graphify.Client.Model.Enums;
using Graphify.Client.View.Drawing;
using Graphify.Client.ViewModel;
using Microsoft.Win32;
using ReactiveUI;
using SharpGL;
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

    }    
  
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
        ViewModel?.SetEditMode.Execute(EditMode.CreateCircleTwoPoints);
    }

    private void CreateCurveModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        //ViewModel?.SetEditMode.Execute(EditMode.CreateCurve);
    }
    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        SaveFileDialog exportFileDialog = new SaveFileDialog
        {
            FileName = "test.svg",
            DefaultExt = ".svg",
            Filter = "SVG image (*.svg)|*.svg|PNG image (*.png)|*.png|Grafify image (*.grafify)|*.grafify",
            InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
            CheckFileExists = false
        };

        if (exportFileDialog.ShowDialog() != true)
        {
            return;
        }

        string filePath = exportFileDialog.FileName;
        string selectedExtension = Path.GetExtension(filePath);
        ExportFileType fileType = SelectFileType(selectedExtension);
        ViewModel?.Export.Execute((filePath, fileType));
    }

    // что возвращать, если пришла белиберда? TODO
    private ExportFileType SelectFileType(string selectedExtension)
    {
        ExportFileType fileType = selectedExtension switch
        {
            ".svg" => ExportFileType.Svg,
            ".png" => ExportFileType.Png,
            ".grafify" => ExportFileType.Custom,
            _ => throw new InvalidOperationException(selectedExtension)
        };
        return fileType;
    }

    private void UndoButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.Undo.Execute();
    }

    private void RedoButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.Redo.Execute();
    }

    private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ZoomInButton_Click(object sender, RoutedEventArgs e)
    {

    }
    private void ObjectOptionsButton_Click(object sender, RoutedEventArgs e)
    {
        ContextMenu cm = this.FindResource("ObjectOptionsButton") as ContextMenu;
        cm.PlacementTarget = sender as Button;
        cm.IsOpen = true;
    }
    private void DeleteObjectButton_Click(object sender, RoutedEventArgs e)
    { }
    private void CloneObjectButton_Click(object sender, RoutedEventArgs e)
    { }
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

    private void GlWindow_MouseMove(object sender, MouseEventArgs args)
    {
        if (ViewModel is null)
        {
            return;
        }
        
        var position = args.GetPosition((OpenGLControl)sender);
        position.X -= GlWindow.ActualWidth / 2;
        position.Y = GlWindow.ActualHeight / 2 - position.Y;
        ViewModel.MouseMove.Execute(new Vector2((float)position.X, (float)position.Y));
    }
}
