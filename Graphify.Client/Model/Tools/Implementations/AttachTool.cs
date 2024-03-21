using System.Numerics;
using Graphify.Client.Model.Commands;
using Graphify.Client.Model.Interfaces;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Client.Model.Tools.Implementations;

public class AttachDetachTool : IApplicationTool
{
    private readonly ApplicationContext _applicationContext;
    private readonly CommandsBuffer _commandsBuffer;

    private bool ReadyToAttach => PointSelected && FigureSelected;
    private bool NothingSelected => !PointSelected && !FigureSelected;
    private bool PointSelected => _point is not null;
    private bool FigureSelected => _figure is not null;

    private Point? _point;
    private IFigure? _figure;

    public AttachDetachTool(ApplicationContext applicationContext, CommandsBuffer commandsBuffer)
    {
        _applicationContext = applicationContext;
        _commandsBuffer = commandsBuffer;
    }

    public void RightMouseDown(Vector2 clickPosition) { }

    public void RightMouseUp(Vector2 clickPosition) { }

    public void MouseMove(Vector2 newPosition) { }

    public void MouseDown(Vector2 clickPosition)
    {
        var pointWasSelected = PointSelected;
        var figureWasSelected = FigureSelected;

        if (NothingSelected)
        {
            if (!TrySelectPointAt(clickPosition))
            {
                TrySelectFigureAt(clickPosition);
            }
        }

        if (PointSelected && !FigureSelected && _point!.IsAttached)
        {
            try
            {
                _point.Detach();
            }
            finally
            {
                UnselectPoint();
            }
            return;
        }

        if (pointWasSelected)
        {
            if (!TrySelectFigureAt(clickPosition))
            {
                UnselectPoint();
            }
        }
        if (figureWasSelected)
        {
            if (!TrySelectPointAt(clickPosition))
            {
                UnselectFigure();
            }
        }

        if (!ReadyToAttach)
        {
            return;
        }

        try
        {
            _point!.AttachTo(_figure!);
        }
        finally
        {
            UnselectFigure();
            UnselectPoint();
        }
    }

    public void MouseUp(Vector2 clickPosition)
    {

    }

    public bool InProgress()
    {
        return false;
    }

    public void Cancel() { }

    public void OnToolChanged()
    {
    }

    private void TrySelectClosestObject(Vector2 clickPosition)
    {
        var closestObject = _applicationContext.Surface.TryGetClosestObject(clickPosition);
        if (closestObject is Point point)
        {
            SelectPoint(point);
        }
        else if (closestObject is IFigure figure)
        {
            SelectFigure(figure);
        }
    }

    private bool TrySelectPointAt(Vector2 clickPosition)
    {
        var point = _applicationContext.Surface.TryGetClosestPoint(clickPosition);

        if (point is null)
        {
            return false;
        }

        if (_point == point)
        {
            UnselectPoint();
        }
        else
        {
            SelectPoint(point);
        }

        return true;
    }

    private bool TrySelectFigureAt(Vector2 clickPosition)
    {
        var figure = _applicationContext.Surface.TryGetClosestFigure(clickPosition);

        if (figure is null)
        {
            return false;
        }

        if (_figure == figure)
        {
            UnselectPoint();
        }
        else
        {
            SelectFigure(figure);
        }

        return true;
    }

    private void UnselectPoint()
    {
        if (!PointSelected)
        {
            throw new InvalidOperationException("Точка не выделена");
        }

        _point!.ObjectState = ObjectState.Default;
        _point = null;
    }

    private void UnselectFigure()
    {
        if (!FigureSelected)
        {
            throw new InvalidOperationException("Фигура не выделена");
        }

        _figure!.ObjectState = ObjectState.Default;
        _figure = null;
    }

    private void SelectPoint(Point point)
    {
        _point = point;
        _point.ObjectState = ObjectState.ControlPoint;
    }

    private void SelectFigure(IFigure figure)
    {
        _figure = figure;
        _figure.ObjectState = ObjectState.Selected;
    }
}
