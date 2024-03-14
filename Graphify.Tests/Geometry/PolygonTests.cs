using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Polygons;
using Graphify.Geometry.GeometricObjects.Points;
using System.Numerics;

namespace Graphify.Tests.Geometry
{
    internal class PolygonTests
    {
        private Polygon _polygon = null;
        private Polygon _secondPolygon = null;
        private Point _attachedPoint = null;
        private readonly IEqualityComparer<Polygon> _comparer = new PolygonComparer();
        private Point[] points = null;

        [SetUp]
        public void Setup()
        {
            points = [new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(0, 0)];
            _polygon = new Polygon(points);
            _attachedPoint = new Point(0, 0);
            _secondPolygon = new Polygon([new Point(-1, 0), _attachedPoint, new Point(0, -1), new Point(-1, -1), new Point(-1, 0)]);
            _attachedPoint.AttachTo(_polygon);
        }


        //IsNextTo
        [TestCaseSource(nameof(bigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _polygon.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        private static readonly object[] bigDistance =
        {
            new object[] { new Vector2(3f, 0f), 1f },
        };

        [TestCaseSource(nameof(smallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _polygon.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        private static readonly object[] smallDistance =
        {
            new object[] { new Vector2(2f, 0f), 2f },
        };

        [TestCaseSource(nameof(wrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _polygon.IsNextTo(point, distance));
        }

        private static readonly object[] wrongDataIsNextTo =
        {
            new object[] { new Vector2(2f, 0f), -1f },
        };

        //Move
        // тесты не проходят возможно тк первая точка есть в первой и последней прямой и она двигается 2 раза
        [TestCaseSource(nameof(moveData))]
        public void GIVEN_Line_WHEN_move_THEN_expected_new_coords(Vector2 shift, Polygon expected)
        {
            _polygon.Move(shift);
            Polygon actual = _polygon;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] moveData =
        {
            new object[] { new Vector2(1f, 1f), new Polygon([new Point(1f, 1f), new Point(1f, 2f), new Point(2f, 2f), new Point(2f, 1f), new Point(1f, 1f)]) },
            new object[] { new Vector2(2f, -2f), new Polygon([new Point(2, -2), new Point(2, -1), new Point(3, -1), new Point(3, -2), new Point(2, -2)]) },
            new object[] { new Vector2(0, 0), new Polygon([new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(0, 0)]) },
        };

        [TestCaseSource(nameof(attachedPointMoveData))]
        public void GIVEN_Line_WHEN_move_attached_point_THEN_expected_exception(Vector2 shift)
        {
            Assert.Throws<InvalidOperationException>(() => _secondPolygon.Move(shift));
        }

        private static readonly object[] attachedPointMoveData =
        {
            new object[] { new Vector2(1f, 1f)},
        };
    }
}
