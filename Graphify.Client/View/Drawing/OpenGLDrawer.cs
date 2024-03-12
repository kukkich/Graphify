using System.Numerics;
using Graphify.Client.View.Drawing.Base;
using Graphify.Client.View.Drawing.BezierCurve;
using Graphify.Client.View.Drawing.Circle;
using Graphify.Client.View.Drawing.Line;
using Graphify.Client.View.Drawing.Point;
using Graphify.Client.View.Drawing.Polygon;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Points;
using SharpGL;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace Graphify.Client.View.Drawing;

public class OpenGLDrawer : IDrawer
{
    public bool GlInitialized => _gl is not null;

    private OpenGL _gl;
    private IBaseDrawer _defaultDrawer;
    
    private IGeometryObjectDrawer<IEnumerable<Vector2>> _currentBezierCurveDrawer;
    private IGeometryObjectDrawer<(Vector2, float)> _currentCircleDrawer;
    private IGeometryObjectDrawer<(Vector2, Vector2)> _currentLineDrawer;
    
    private readonly Dictionary<PointVariant, IGeometryObjectDrawer<Vector2>> _pointVariantDrawers = [];
    private IGeometryObjectDrawer<Vector2> _currentPointDrawer;
    
    private IGeometryObjectDrawer<IEnumerable<Vector2>> _currentPolygonDrawer;

    public DrawSettings Settings { get; private set; }

    public void InitGl(OpenGL gl)
    {
        _gl = gl;
        Settings = new DrawSettings();
        
        _defaultDrawer = new OpenGLDefaultDrawer(gl);
        
        _pointVariantDrawers.Add(PointVariant.Circle, new PointCircleDrawer(_defaultDrawer));
        _pointVariantDrawers.Add(PointVariant.Cross, new PointCrossDrawer(_defaultDrawer));

        _currentBezierCurveDrawer = new BaseBezierCurveDrawer(_defaultDrawer);
        _currentPolygonDrawer = new BasePolygonDrawer(_defaultDrawer);
        _currentCircleDrawer = new BaseCircleDrawer(_defaultDrawer);
        _currentLineDrawer = new BaseLineDrawer( _defaultDrawer);
    }

    public void Reset()
    {
        _gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
    }
    
    public void DrawBezierCurve(IEnumerable<Vector2> points)
    {
        _currentBezierCurveDrawer.Draw(points, Settings);
    }

    public void DrawCircle(Vector2 center, float radius)
    {
        _currentCircleDrawer.Draw((center, radius), Settings);
    }

    public void DrawLine(Vector2 start, Vector2 end)
    {
        _currentLineDrawer.Draw((start, end), Settings);
    }
    
    public void DrawPoint(Vector2 point)
    {
        _currentPointDrawer = _pointVariantDrawers[Settings.PointVariant];
        _currentPointDrawer.Draw(point, Settings);
    }

    public void DrawPolygon(IEnumerable<Vector2> points)
    {
        _currentPolygonDrawer.Draw(points, Settings);
    }
}
