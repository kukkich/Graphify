using System.Numerics;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;
//using System.Drawing;

namespace Graphify.Tests.Geometry
{
    internal class LineTests
    {
        private Line _line = null!;
        private readonly IEqualityComparer<Line> _comparer = new LineComparer();
        private Line _secondLine = null!;
        private Point _b = null!;
        private static Point pointA = null!;
        private Point _pointB = null!;

        [SetUp]
        public void Setup()
        {
            pointA = new Point(0, 0);
            _pointB = new Point(1, 1);
            _line = new Line(pointA, _pointB);
            _b = new Point(2, 1);
            _secondLine = new Line(new Point(0, 1), _b);
            //_secondLine.ConsumeAttach(_b);
            _b.AttachTo(_line);
        }

        //IsNextTo
        [TestCaseSource(nameof(BigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _line.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        private static readonly object[] BigDistance =
        [
            new object[] { new Vector2(1.5f, -0.5f), 1f }
        ];

        [TestCaseSource(nameof(SmallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _line.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        private static readonly object[] SmallDistance =
        [
            new object[] { new Vector2(1, 0), 1 }
        ];

        [TestCaseSource(nameof(WrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _line.IsNextTo(point, distance));
        }

        private static readonly object[] WrongDataIsNextTo =
        [
            new object[] { new Vector2(0.5f, 0.5f), -1f }
        ];

        //Move
        [TestCaseSource(nameof(MoveData))]
        public void GIVEN_Line_WHEN_move_THEN_expected_new_coords(Vector2 shift, Line expected)
        {
            _line.Move(shift);
            Line actual = _line;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] MoveData =
        [
            new object[] { new Vector2(0.5f, 0.5f), new Line(new Point(0.5f, 0.5f), new Point(1.5f, 1.5f))},
            new object[] { new Vector2(-0.5f, -0.5f), new Line(new Point(-0.5f, -0.5f), new Point(0.5f, 0.5f))},
            new object[] { new Vector2(0, 0), new Line(new Point(0, 0), new Point(1, 1))}
        ];

        [TestCaseSource(nameof(AttachedPointMoveData))]
        public void GIVEN_Line_WHEN_move_attached_point_THEN_expected_exception(Vector2 shift, Line expected)
        {
            Assert.Throws<InvalidOperationException>(() => _secondLine.Move(shift));
        }

        private static readonly object[] AttachedPointMoveData =
        [
            new object[] { new Vector2(0.5f, 0.5f), new Line(new Point(0.5f, 0.5f), new Point(1.5f, 1.5f))}
        ];

        //Rotate
        //функция не работает
        [TestCaseSource(nameof(RotateData))]
        public void GIVEN_Line_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Line expected)
        {
            _line.Rotate(shift, angle);
            Line actual = _line;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] RotateData =
        [
            new object[] { new Point(1, 0), 90, new Line(new Point(1, 1), new Point(2, 0))},
            new object[] { new Point(1, 0), 180, new Line(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), -90, new Line(new Point(0, 0), new Point(-1, 1))}
        ];

        [TestCaseSource(nameof(AttachedPointRotateData))]
        public void GIVEN_Line_WHEN_rotate_attached_point_THEN_expected_exception(Point shift, float angle, Line expected)
        {
            Assert.Throws<InvalidOperationException>(() => _secondLine.Rotate(shift, angle));
        }

        private static readonly object[] AttachedPointRotateData =
        [
            new object[] { new Point(1, 0), 90, new Line(new Point(1, 1), new Point(2, 0))}
        ];

        //Reflect
        //функция которая делает Rotate на 180 градусов 
        //не нужно ли нам сделать отражение относительно прямой вместо этого
        [TestCaseSource(nameof(ReflectData))]
        public void GIVEN_Line_WHEN_reflect_THEN_expected_new_coords(Point point, Line expected)
        {
            _line.Reflect(point);
            Line actual = _line;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] ReflectData =
        [
            new object[] { new Point(1, 0), new Line(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), new Line(new Point(0, 0), new Point(-1, -1))},
            new object[] { new Point(1, 2), new Line(new Point(2, 4), new Point(1, 3))}
        ];

        [TestCaseSource(nameof(AttachedPointReflectData))]
        public void GIVEN_Line_WHEN_reflect_attached_point_THEN_expected_exception(Point point, Line expected)
        {
            Assert.Throws<InvalidOperationException>(() => _secondLine.Reflect(point));
        }

        private static readonly object[] AttachedPointReflectData =
        [
            new object[] { new Point(1, 0), new Line(new Point(2, 0), new Point(1, -1))}
        ];

        //ConsumeAttach
        [TestCaseSource(nameof(AttachedData))]
        public void GIVEN_Line_WHEN_the_point_is_attached_THEN_expected_true(Point attachable)
        {
            _line.Attach(attachable);
            bool result = _line.Attached.Select(x => x.Object).Contains(attachable);
            Assert.That(result, Is.True);
        }

        private static readonly object[] AttachedData =
        [
            new object[] {new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 1)}
        ];

        [TestCaseSource(nameof(ControlPointToAttachedData))]
        public void GIVEN_Line_WHEN_the_control_point_is_attached_THEN_expected_exception(Point attachable)
        {
            Assert.Throws<InvalidOperationException>(() => _line.Attach(attachable));
        }

        private static readonly object[] ControlPointToAttachedData =
        [
            new object[] {new Point(0, 0)},
            new object[] {new Point(1, 1)}
        ];

        [TestCaseSource(nameof(DoubleAttachedData))]
        public void GIVEN_Line_WHEN_the_attached_point_is_attached_THEN_expected_exception(Point attachable)
        {
            _line.Attach(attachable);
            Assert.Throws<InvalidOperationException>(() => _line.Attach(attachable));
        }

        private static readonly object[] DoubleAttachedData =
        [
            new object[] {new Point(0.5f, 0.5f)}
        ];

        //ConsumeDetach
        [TestCaseSource(nameof(DetachData))]
        public void GIVEN_Line_WHEN_the_point_is_detached_THEN_expected_true(Point dettachable)
        {
            _line.Attach(dettachable);

            _line.Detach(dettachable);
            bool result = !_line.Attached.Select(x => x.Object).Contains(dettachable);
            Assert.That(result, Is.True);
        }

        private static readonly object[] DetachData =
        [
            new object[] { new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 0)}
        ];
    }
}
