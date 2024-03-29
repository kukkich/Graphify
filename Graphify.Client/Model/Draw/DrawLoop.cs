using System.Diagnostics;
using System.Windows.Threading;
using Graphify.Client.View.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;

namespace Graphify.Client.Model.Draw;

// TODO refactor
public class DrawLoop
{
    private readonly IGeometryContext _context;
    private readonly OpenGLDrawer _drawer;

    private Stopwatch _stopwatch = null!;
    private float _fps;
    private bool _isRunning;

    private DispatcherTimer _timer = null!;

    public DrawLoop(ApplicationContext applicationContext, OpenGLDrawer drawer)
    {
        _drawer = drawer;
        _context = applicationContext.Surface;
    }

    public void Initialize(float fps = 60)
    {
        _fps = fps;
        _stopwatch = new Stopwatch();
        _isRunning = false;

        _timer = new DispatcherTimer();
        _timer.Tick += Timer_Tick!;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        _stopwatch.Start();
        Update(1.0f / _fps);
        _stopwatch.Stop();
        _stopwatch.Reset();
    }

    public void Start()
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;
        TimeSpan targetElapsedTime = TimeSpan.FromSeconds(1.0 / _fps);

        _timer.Interval = targetElapsedTime;
        _timer.Start();
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private void Update(float deltaTime)
    {
        if (!_isRunning || !_drawer.GlInitialized)
        {
            return;
        }

        _drawer.Start();
        foreach (IGeometricObject geometricObject in _context.Objects)
        {
            geometricObject.Draw(_drawer);
        }
        _drawer.End();
    }
}
