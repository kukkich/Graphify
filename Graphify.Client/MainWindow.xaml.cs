using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DynamicData;
using Graphify.Client.Model.Enums;
using Graphify.Client.View.Drawing;
using Graphify.Client.ViewModel;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Microsoft.Win32;
using ReactiveUI;
using SharpGL;
using SharpGL.WPF;
using Graphify.Geometry.GeometricObjects.Points;
using System.Drawing;
using ReactiveUI.Fody.Helpers;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using Graphify.Geometry.GeometricObjects.Interfaces;
using DynamicData;
using System;

namespace Graphify.Client;

public partial class MainWindow
{
    private readonly OpenGLDrawer _drawer;
    private OpenGL _gl;
    ReadOnlyObservableCollection<IGeometricObject> geometricObjects;
    public ReadOnlyObservableCollection<IGeometricObject> GeometricObjects => geometricObjects;
    public MainWindow(AppViewModel viewModel, OpenGLDrawer drawer)
    {
        _drawer = drawer;
        ViewModel = viewModel;
        DataContext = viewModel;
        var todispose = this.ViewModel.GeometryObjects.Connect().Bind(out geometricObjects)
        .Subscribe();
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

            todispose.DisposeWith(disposables);
            
            foreach (var command in ViewModel.AllCommands)
            {
                command.ThrownExceptions.Subscribe(HandleError)
                    .DisposeWith(disposables);
            }
        });

        this.listGeometryObjects.DataContext = viewModel;
        TopPanel.DataContext = viewModel;
        this.WhenAnyValue(x => x.Height)
            .Subscribe(height =>
            {
                listGeometryObjects.Height = Math.Max(0, height - 85);
            });
    }

    private void ObjectOptionsButton_Click(object sender, RoutedEventArgs e)
    {
        ContextMenu cm = this.FindResource("ObjectOptions") as ContextMenu;
        cm.PlacementTarget = sender as Button;
        cm.IsOpen = true;
    }

    private void HandleError(Exception e)
    {
        MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

    private void AttachePointModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.SetEditMode.Execute(EditMode.AttachDetach);
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
        ViewModel?.SetEditMode.Execute(EditMode.CreateBezierCurve);
    }
    private void RotateModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.SetEditMode.Execute(EditMode.Rotate);
    }
    private void ReflectModeButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.SetEditMode.Execute(EditMode.Reflect);
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
  
    private void ImportButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }
        ViewModel?.OpenImportDialogCommand.Execute();
    }

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

    private void GlWindow_MouseUp(object sender, MouseButtonEventArgs args)
    {
        if (ViewModel is null)
        {
            return;
        }

        var position = args.GetPosition((OpenGLControl)sender);
        position.X -= GlWindow.ActualWidth / 2;
        position.Y = GlWindow.ActualHeight / 2 - position.Y;
        ViewModel.MouseUp.Execute(new Vector2((float)position.X, (float)position.Y));
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

    private void EditObjectButton_Click(object sender, RoutedEventArgs e)
    {
    }    
    private void DeleteObjectButton_Click(object sender, RoutedEventArgs e)
    { }
    private void CloneObjectButton_Click(object sender, RoutedEventArgs e)
    {
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

    private void CleanAllModeButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.Clean.Execute();
    }
}
