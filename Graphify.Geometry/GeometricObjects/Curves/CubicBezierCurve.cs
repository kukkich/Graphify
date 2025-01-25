using System.Numerics;
using Graphify.Geometry.Attachment;
using Graphify.Geometry.Drawing;
using Graphify.Geometry.Export;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Graphify.Geometry.GeometricObjects.Curves;

public class CubicBezierCurve : Curve
{
    public override int RequiredPointsCount => 4;

    private Vector2 CurveFunction(float t)
    {
        Span<float> tc =
        [
            (1f-t)*(1f-t)*(1f-t),
            3f*t*(1f-t)*(1f-t),
            3f*t*t*(1f-t),
            t*t*t
        ];
        Vector2 p = new Vector2 { X = 0f, Y = 0f };
        for (int i = 0; i < 4; i++)
        {
            p.X += tc[i] * Points[i].X;
            p.Y += tc[i] * Points[i].Y;
        }
        return p;
    }

    public CubicBezierCurve(Point[] points, CurveStyle? style = null)
        : base(points, style) 
    { }

    public override void Update()
    {
        foreach (var attachedPoint in Attached.ToList())
        {
            var point = attachedPoint.Object;
            var t = attachedPoint.T;
            var newPos = CurveFunction(t);
            var dV = new Vector2(newPos.X - point.X, newPos.Y - point.Y);

            point.Move(dV);
        }
    }

    public override void Attach(Point attachable)
    {
        if (ControlPoints.Contains(attachable))
        {
            throw new InvalidOperationException("Нельзя присоединить точку к данной фигуре: точка является опорной для данной фигуры");
        }

        if (Attached.Find(x => x.Object == attachable) != null)
        {
            throw new InvalidOperationException("Нельзя присоединить точку к данной фигуре: точка уже присоединена к данной фигуре");
        }

        // Todo Linear Search. Should use optimization method through minimizing ||S(t) - Point||
        Vector2 minV = new Vector2();
        float minDst = float.PositiveInfinity;
        float minT = -1f;
        for (float t = 0f; t < 1f; t += 0.01f)
        {
            var point = CurveFunction(t);
            var distV = new Vector2(point.X - attachable.X, point.Y - attachable.Y);
            var dist = distV.Length();
            if (!(dist < minDst))
            {
                continue;
            }

            minV = distV;
            minDst = dist;
            minT = t;
        }

        var attachedPoint = new AttachedPoint(attachable, minT);
        Attached.Add(attachedPoint);
        attachable.Move(minV);
    }

    public override bool IsNextTo(Vector2 point, float distance)
    {
        if (distance <= 0)
        {
            throw new ArgumentException($"Значение distance должно быть строго положительным числом. Ожидалось: distance > 0, получено: {distance}");
        }

        // Todo Linear Search. Should use optimization method through minimizing ||S(t) - Point||
        for (float t = 0f; t < 1f; t += 0.01f)
        {
            var curPoint = CurveFunction(t);
            var distV = new Vector2(point.X - curPoint.X, point.Y - curPoint.Y);
            if (distV.Length() < distance)
            {
                return true;
            }
        }

        return false;
    }

    public override void Draw(IDrawer drawer)
    {
        if (!Style.Visible)
        {
            return;
        }
        Style.ApplyStyle(drawer);

        var points = ControlPoints.Select(point => new Vector2(point.X, point.Y))
            .ToList();

        drawer.DrawBezierCurve(points, ObjectState);
    }

    public override FigureExportData GetExportData()
    {
        var exportData = new FigureExportData
        {
            FigureType = ObjectType.CubicBezier,
            Style = Style,
            LeftBottomBound = new Vector2(
                ControlPoints.Min(p => p.X),
                ControlPoints.Min(p => p.Y)
            ),
            RightTopBound = new Vector2(
                ControlPoints.Max(p => p.X),
                ControlPoints.Max(p => p.Y)
            )
        };

        return exportData;
    }

    public override IGeometricObject Clone() => throw new NotImplementedException();
}
