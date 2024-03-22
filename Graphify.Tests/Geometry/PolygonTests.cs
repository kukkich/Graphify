using System.Numerics;
using Graphify.Geometry.GeometricObjects.Points;
using Graphify.Geometry.GeometricObjects.Polygons;

namespace Graphify.Tests.Geometry
{
    internal class PolygonTests
    {
        private Polygon _polygon = null;
        private Polygon _secondPolygon = null;
        private Point _attachedPoint = null;
        private readonly IEqualityComparer<Polygon> _comparer = new PolygonComparer();
        private Point[] _points = null;

        [SetUp]
        public void Setup()
        {
            _points = [new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(0, 0)];
            _polygon = new Polygon(_points);
            _attachedPoint = new Point(0, 0);
            _secondPolygon = new Polygon([new Point(-1, 0), _attachedPoint, new Point(0, -1), new Point(-1, -1), new Point(-1, 0)]);
            _attachedPoint.AttachTo(_polygon);
        }


        //IsNextTo
        [TestCaseSource(nameof(BigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _polygon.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        private static readonly object[] BigDistance =
        [
            new object[] { new Vector2(3f, 0f), 1f }
        ];

        [TestCaseSource(nameof(SmallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _polygon.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        private static readonly object[] SmallDistance =
        [
            new object[] { new Vector2(2f, 0f), 2f }
        ];

        [TestCaseSource(nameof(WrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _polygon.IsNextTo(point, distance));
        }

        private static readonly object[] WrongDataIsNextTo =
        [
            new object[] { new Vector2(2f, 0f), -1f }
        ];

        //Move
        // тесты не проходят возможно тк первая точка есть в первой и последней прямой и она двигается 2 раза
        [TestCaseSource(nameof(MoveData))]
        public void GIVEN_Polygon_WHEN_move_THEN_expected_new_coords(Vector2 shift, Polygon expected)
        {
            _polygon.Move(shift);
            Polygon actual = _polygon;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] MoveData =
        [
            new object[] { new Vector2(1f, 1f), new Polygon([new Point(1f, 1f), new Point(1f, 2f), new Point(2f, 2f), new Point(2f, 1f), new Point(1f, 1f)]) },
            new object[] { new Vector2(2f, -2f), new Polygon([new Point(2, -2), new Point(2, -1), new Point(3, -1), new Point(3, -2), new Point(2, -2)]) },
            new object[] { new Vector2(0, 0), new Polygon([new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(0, 0)]) }
        ];

        [TestCaseSource(nameof(AttachedPointMoveData))]
        public void GIVEN_Polygon_WHEN_move_attached_point_THEN_expected_exception(Vector2 shift)
        {
            Assert.Throws<InvalidOperationException>(() => _secondPolygon.Move(shift));
        }

        private static readonly object[] AttachedPointMoveData =
        [
            new object[] { new Vector2(1f, 1f)}
        ];


        //ConsumeAttach
        [TestCaseSource(nameof(AttachedData))]
        public void GIVEN_Polygon_WHEN_the_point_is_attached_THEN_expected_true(Point attachable)
        {
            _polygon.ConsumeAttach(attachable);
            bool result = _polygon.Attached.Contains(attachable);
            Assert.That(result, Is.True);
        }

        private static readonly object[] AttachedData =
        [
            new object[] {new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 1)}
        ];

        [TestCaseSource(nameof(ControlPointToAttachedData))]
        public void GIVEN_Polygon_WHEN_the_control_point_is_attached_THEN_expected_exception(Point attachable)
        {
            Assert.Throws<InvalidOperationException>(() => _polygon.ConsumeAttach(attachable));
        }

        private static readonly object[] ControlPointToAttachedData =
            [];

        [TestCaseSource(nameof(DoubleAttachedData))]
        public void GIVEN_Polygon_WHEN_the_attached_point_is_attached_THEN_expected_exception(Point attachable)
        {
            _polygon.ConsumeAttach(attachable);
            Assert.Throws<InvalidOperationException>(() => _polygon.ConsumeAttach(attachable));
        }

        private static readonly object[] DoubleAttachedData =
        [
            new object[] {new Point(0.5f, 0.5f)}
        ];

        //ConsumeDetach
        [TestCaseSource(nameof(DetachData))]
        public void GIVEN_Polygon_WHEN_the_point_is_detached_THEN_expected_true(Point dettachable)
        {
            _polygon.ConsumeAttach(dettachable);

            _polygon.ConsumeDetach(dettachable);
            bool result = true;
            if (_polygon.Attached.Contains(dettachable))
                result = false;
            Assert.That(result, Is.True);
        }

        private static readonly object[] DetachData =
        [
            new object[] { new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 0)}
        ];


        //Rotate
        //аналогично move
        [TestCaseSource(nameof(RotateData))]
        public void GIVEN_Polygon_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Polygon expected)
        {
            _polygon.Rotate(shift, angle);
            Polygon actual = _polygon;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] RotateData =
        [
            new object[] { new Point(1, 1), 90, new Polygon([new Point(0, 2), new Point(1, 2), new Point(1, 1), new Point(0, 1), new Point(0, 2)]) }
        ];

        [TestCaseSource(nameof(AttachedPointRotateData))]
        public void GIVEN_Polygon_WHEN_rotate_attached_point_THEN_expected_exception(Point shift, float angle)
        {
            Assert.Throws<InvalidOperationException>(() => _secondPolygon.Rotate(shift, angle));
        }

        private static readonly object[] AttachedPointRotateData =
        [
            new object[] { new Point(1, 0), 90}
        ];

        //Reflect
        [TestCaseSource(nameof(ReflectData))]
        public void GIVEN_Polygon_WHEN_reflect_THEN_expected_new_coords(Point point, Polygon expected)
        {
            _polygon.Reflect(point);
            Polygon actual = _polygon;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] ReflectData =
        [
            new object[] { new Point(2, 2), new Polygon([new Point(4, 4), new Point(4, 3), new Point(3, 3), new Point(3, 4), new Point(4, 4)]) }
        ];

        [TestCaseSource(nameof(AttachedPointReflectData))]
        public void GIVEN_Polygon_WHEN_reflect_attached_point_THEN_expected_exception(Point point)
        {
            Assert.Throws<InvalidOperationException>(() => _secondPolygon.Reflect(point));
        }

        private static readonly object[] AttachedPointReflectData =
        [
            new object[] { new Point(1, 0)}
        ];
    }
}
