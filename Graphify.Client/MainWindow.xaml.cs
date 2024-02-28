using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL.WPF;
using SharpGL;
using System.Windows.Controls;
using System.Windows;

namespace Graphify.Client;

public partial class MainWindow     
{    
    private OpenGL _gl;
    public MainWindow(AppViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
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
    {
        
    }
    private void Clone_Click(object sender, RoutedEventArgs e)
    {

    }    
    public class Figure //temporarily
    {
        public string Data { get; set; }
        public string Type { get; set; }
    }
}
