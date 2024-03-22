using System.Numerics;
using System.Windows.Input;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Point = Graphify.Geometry.GeometricObjects.Points.Point;

namespace Graphify.Client.Model.Tools.Implementations;

public class RotateTool : IApplicationTool
{
    private const float SmallAngle = 0.1f;

    private readonly ApplicationContext _applicationContext;
    private readonly CommandsBuffer _commandsBuffer;

    private Vector2 _previousMousePosition;
    private Point? _point;
    private float _angle;

    public RotateTool(ApplicationContext applicationContext, CommandsBuffer commandsBuffer)
    {
        _applicationContext = applicationContext;
        _commandsBuffer = commandsBuffer;
    }

    public void RightMouseDown(Vector2 clickPosition) { }

    public void RightMouseUp(Vector2 clickPosition) { }

    public void MouseMove(Vector2 newPosition)
    {
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            if (_point is null)
            {
                return;
            }

            var alpha = CalculateAngle(newPosition);

            if (Math.Abs(alpha) >= SmallAngle)
            {
                _angle += alpha;

                Rotate(_point, alpha);

                if (Math.Abs(_angle) > 360d)
                {
                    _angle = (float)Math.IEEERemainder(_angle, 360d);
                }
            }
        }

        _previousMousePosition = newPosition;
    }

    private void Rotate(Point point, float angle)
    {
        foreach (var geometricObject in _applicationContext.SelectedObjects)
        {
            geometricObject.Rotate(point, angle);
        }
    }

    public void MouseDown(Vector2 clickPosition)
    {
        IGeometricObject closestObject = _applicationContext.Surface.TryGetClosestObject(clickPosition);

        if (!Keyboard.IsKeyDown(Key.LeftCtrl))
        {
            if (closestObject is Point point)
            {
                if (_point == point)
                {
                    _point.ObjectState = ObjectState.Default;
                    _point = null;
                }
                else
                {
                    if (_point == null)
                    {
                        _point = point;
                        _point.ObjectState = ObjectState.ControlPoint;
                    }
                }
            }
        }
        else
        {
            if (closestObject != null)
            {
                if (_applicationContext.SelectedObjects.Contains(closestObject) && closestObject != _point)
                {
                    _applicationContext.UnSelect(closestObject);
                    return;
                }

                _applicationContext.Select(closestObject, false);
            }
            else
            {
                _applicationContext.ClearSelected();
            }
        }
    }

    public void MouseUp(Vector2 clickPosition)
    {
        if (!(Math.Abs(_angle) >= SmallAngle) || !_applicationContext.SelectedObjects.Any()) return;

        _commandsBuffer.AddCommand(new RotateCommand(_applicationContext.SelectedObjects, _point, _angle));
        _angle = 0;
    }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel() { }

    public void OnToolChanged()
    {
        if (_point is null)
        {
            return;
        }

        _point.ObjectState = ObjectState.Default;
        _point = null;
        _applicationContext.ClearSelected();
    }

    private float CalculateAngle(Vector2 newPosition)
    {
        var pointAsVector = new Vector2(_point.X, _point.Y);

        var vector1 = _previousMousePosition - pointAsVector;
        var vector2 = newPosition - pointAsVector;

        var dotProduct = Vector2.Dot(vector1, vector2);
        var length1 = vector1.Length();
        var length2 = vector2.Length();

        var сosAlpha = dotProduct / (length1 * length2);

        if (сosAlpha < -1)
        {
            сosAlpha = -1;
        }

        if (сosAlpha > 1)
        {
            сosAlpha = 1;
        }

        var alpha = Math.Acos(сosAlpha) * 180 / Math.PI;

        var vectorForSign = Vector3.Cross(new Vector3(vector1, 0), new Vector3(vector2, 0));

        alpha *= Math.Sign(vectorForSign.Z);

        return (float)alpha;
    }
}
