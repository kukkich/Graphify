using System.Numerics;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class PointTests
    {
        private Point _controlPoint = null!;
        private Line _lineToAttach = null!;
        private Point _point = null!;
        private Point _attachedPoint = null!;
        private readonly IEqualityComparer<Point> _comparer = new PointComparer();

        [SetUp]
        public void Setup()
        {
            _controlPoint = new Point(1, 0);
            _lineToAttach = new Line(_controlPoint, new Point(-1, 0));
            _point = new Point(0, 0);
            _attachedPoint = new Point(0, 0);
            _attachedPoint.AttachTo(_lineToAttach);
        }

        //IsNextTo
        [TestCaseSource(nameof(LongDistance))]
        public void GIVEN_Point_WHEN_distance_between_points_is_too_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _point.IsNextTo(point, distance);
            Assert.IsFalse(result);
        }

        private static readonly object[] LongDistance =
        [
            new object[] { new Vector2(1f, 2f), 2f },
            new object[] { new Vector2(1f, 2f), 1f }
        ];

        [TestCaseSource(nameof(ShortDistance))]
        public void GIVEN_Point_WHEN_distance_between_points_is_too_short_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _point.IsNextTo(point, distance);
            Assert.IsTrue(result);
        }

        private static readonly object[] ShortDistance =
        [
            new object[] { new Vector2(0.5f, 0.5f), 2f },
            new object[] { new Vector2(0.5f, 0.5f), 1f }
        ];

        [TestCaseSource(nameof(NegativeDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _point.IsNextTo(point, distance));
        }

        private static readonly object[] NegativeDataIsNextTo =
        [
            new object[] { new Vector2(0.5f, 0.5f), -1f }
        ];

        //Move
        [TestCaseSource(nameof(ShiftData))]
        public void GIVEN_Point_WHEN_move_THEN_expected_new_coords(Vector2 shift, Point expected)
        {
            _point.Move(shift);
            Point actual = _point;

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] ShiftData =
        [
            new object[] { new Vector2(0.5f, 0.5f), new Point(0.5f,0.5f) },
            new object[] { new Vector2(-0.5f, -0.5f), new Point(-0.5f, -0.5f) }
        ];

        //Rotate 
        [TestCaseSource(nameof(RotateData))]
        public void GIVEN_Point_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Point expected)
        {
            _point.Rotate(shift, angle);
            Point actual = _point;

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] RotateData =
        [
            new object[] { new Point(1, 0), 90, new Point(1, 1) },
            new object[] { new Point(1, 0), 540, new Point(2, 0) },
            new object[] { new Point(1, 0), -90, new Point(1, -1) }
        ];

        [TestCaseSource(nameof(AttachedRotateData))]
        public void GIVEN_attached_Point_WHEN_rotate_THEN_exception(Point shift, float angle)
        {
            Assert.Throws<InvalidOperationException>(() => _attachedPoint.Rotate(shift, angle));
        }

        private static readonly object[] AttachedRotateData =
        [
            new object[] { new Point(1, 0), 90}
        ];

        //Reflect
        [TestCaseSource(nameof(ReflectData))]
        public void GIVEN_Point_WHEN_reflect_THEN_expected_new_coords(Point point, Point expected)
        {
            _point.Reflect(point);
            Point actual = _point;

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] ReflectData =
        [
            new object[] { new Point(0.5f, 0.5f), new Point(1f, 1f) },
            new object[] { new Point(-0.5f, -0.5f), new Point(-1f, -1f) }
        ];

        [TestCaseSource(nameof(AttachedReflectData))]
        public void GIVEN_attached_Point_WHEN_reflect_THEN_exception(Point point)
        {
            Assert.Throws<InvalidOperationException>(() => _attachedPoint.Reflect(point));
        }

        private static readonly object[] AttachedReflectData =
        [
            new object[] { new Point(0.5f, 0.5f) }
        ];

        //CanAttachTo
        [TestCaseSource(nameof(CanAttachToData))]
        public void GIVEN_attached_Point_WHEN_point_already_attached_THEN_expected_false(Line line)
        {
            bool result = _attachedPoint.CanAttachTo(line);

            Assert.IsFalse(result);
        }

        private static readonly object[] CanAttachToData =
        [
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) }
        ];

        [TestCase()]
        public void GIVEN_control_Point_WHEN_point_is_control_for_line_THEN_expected_false()
        {
            bool result = _controlPoint.CanAttachTo(_lineToAttach);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(CandAttachToData))]
        public void GIVEN_Point_WHEN_can_attach_THEN_expected_true(Line line)
        {
            bool result = _point.CanAttachTo(line);

            Assert.IsTrue(result);
        }

        private static readonly object[] CandAttachToData =
        [
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) }
        ];

        //AttachTo
        [TestCaseSource(nameof(AttachToContainsData))]
        public void GIVEN_Point_WHEN_attach_to_line_THEN_expected_line_contains_point(Line line)
        {
            _point.AttachTo(line);
            bool result = line.Attached.Select(x => x.Object).Contains(_point);

            Assert.IsTrue(result);
        }

        private static readonly object[] AttachToContainsData =
        [
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) },
            new object[] { new Line(new Point(1, 1), new Point(-1, 1)) }
        ];

        [TestCaseSource(nameof(AttachToNewCoordsData))]
        public void GIVEN_Point_WHEN_attach_to_line_THEN_expected_new_coords_if_needed(Line line, Point expected)
        {
            _point.AttachTo(line);

            Assert.That(_point, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] AttachToNewCoordsData =
        [
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)), new Point(0, 0) },
            new object[] { new Line(new Point(1, 1), new Point(-1, 1)), new Point(0, 1) }
        ];

        [TestCaseSource(nameof(AttachToExceptionData))]
        public void GIVEN_Point_WHEN_point_cant_attach_THEN_expected_exception(Line line)
        {
            Assert.Throws<InvalidOperationException>(() => _attachedPoint.AttachTo(line));
        }

        private static readonly object[] AttachToExceptionData =
        [
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) }
        ];

        //Detach
        [TestCase()]
        public void GIVEN_Point_WHEN_detach_from_nothing_THEN_expected_exception()
        {
            Assert.Throws<InvalidOperationException>(() => _point.Detach());
        }

        [TestCase()]
        public void GIVEN_Point_WHEN_detach_from_line_THEN_expected_line_not_contains_point()
        {
            _attachedPoint.Detach();
            bool result = _lineToAttach.Attached.Select(x => x.Object).Contains(_point);

            Assert.IsFalse(result);
        }
    }
}
