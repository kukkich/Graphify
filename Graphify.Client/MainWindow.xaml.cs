using System.Reactive.Disposables;
using Graphify.Client.ViewModel;
using ReactiveUI;
using SharpGL.WPF;
using SharpGL;

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

        List<Figure> listObjects = new List<Figure>()
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
        this.listGeometryObjects.ItemsSource = listObjects;

    }
    public class Figure
    {
        public string Data { get; set; }
        public string Type { get; set; }
    }
}
