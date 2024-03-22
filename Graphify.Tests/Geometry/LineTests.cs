using System.Numerics;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;
//using System.Drawing;

namespace Graphify.Tests.Geometry
{
    internal class LineTests
    {
        private Line _line = null;
        private readonly IEqualityComparer<Line> _comparer = new LineComparer();
        private Line _secondLine = null;
        private Point _b = null;
        private static Point _pointA = null;
        private Point _pointB = null;

        [SetUp]
        public void Setup()
        {
            _pointA = new Point(0, 0);
            _pointB = new Point(1, 1);
            _line = new Line(_pointA, _pointB);
            _b = new Point(2, 1);
            _secondLine = new Line(new Point(0, 1), _b);
            //_secondLine.ConsumeAttach(_b);
            _b.AttachTo(_line);
        }

        //IsNextTo
        [TestCaseSource(nameof(bigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _line.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        private static readonly object[] bigDistance =
        {
            new object[] { new Vector2(1.5f, -0.5f), 1f },
        };

        [TestCaseSource(nameof(smallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _line.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        private static readonly object[] smallDistance =
        {
            new object[] { new Vector2(1, 0), 1 },
        };

        [TestCaseSource(nameof(wrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _line.IsNextTo(point, distance));
        }

        private static readonly object[] wrongDataIsNextTo =
        {
            new object[] { new Vector2(0.5f, 0.5f), -1f },
        };

        //Move
        [TestCaseSource(nameof(moveData))]
        public void GIVEN_Line_WHEN_move_THEN_expected_new_coords(Vector2 shift, Line expected)
        {
            _line.Move(shift);
            Line actual = _line;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] moveData =
        {
            new object[] { new Vector2(0.5f, 0.5f), new Line(new Point(0.5f, 0.5f), new Point(1.5f, 1.5f))},
            new object[] { new Vector2(-0.5f, -0.5f), new Line(new Point(-0.5f, -0.5f), new Point(0.5f, 0.5f))},
            new object[] { new Vector2(0, 0), new Line(new Point(0, 0), new Point(1, 1))},
        };

        [TestCaseSource(nameof(attachedPointMoveData))]
        public void GIVEN_Line_WHEN_move_attached_point_THEN_expected_exception(Vector2 shift, Line expected)
        {
            Assert.Throws<InvalidOperationException>(() => _secondLine.Move(shift));
        }

        private static readonly object[] attachedPointMoveData =
        {
            new object[] { new Vector2(0.5f, 0.5f), new Line(new Point(0.5f, 0.5f), new Point(1.5f, 1.5f))},
        };

        //Rotate
        //функция не работает
        [TestCaseSource(nameof(rotateData))]
        public void GIVEN_Line_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Line expected)
        {
            _line.Rotate(shift, angle);
            Line actual = _line;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] rotateData =
        {
            new object[] { new Point(1, 0), 90, new Line(new Point(1, 1), new Point(2, 0))},
            new object[] { new Point(1, 0), 180, new Line(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), -90, new Line(new Point(0, 0), new Point(-1, 1))},
        };

        [TestCaseSource(nameof(attachedPointRotateData))]
        public void GIVEN_Line_WHEN_rotate_attached_point_THEN_expected_exception(Point shift, float angle, Line expected)
        {
            Assert.Throws<InvalidOperationException>(() => _secondLine.Rotate(shift, angle));
        }

        private static readonly object[] attachedPointRotateData =
        {
            new object[] { new Point(1, 0), 90, new Line(new Point(1, 1), new Point(2, 0))},
        };

        //Reflect
        //функция которая делает Rotate на 180 градусов 
        //не нужно ли нам сделать отражение относительно прямой вместо этого
        [TestCaseSource(nameof(reflectData))]
        public void GIVEN_Line_WHEN_reflect_THEN_expected_new_coords(Point point, Line expected)
        {
            _line.Reflect(point);
            Line actual = _line;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] reflectData =
        {
            new object[] { new Point(1, 0), new Line(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), new Line(new Point(0, 0), new Point(-1, -1))},
            new object[] { new Point(1, 2), new Line(new Point(2, 4), new Point(1, 3))}
        };

        [TestCaseSource(nameof(attachedPointReflectData))]
        public void GIVEN_Line_WHEN_reflect_attached_point_THEN_expected_exception(Point point, Line expected)
        {
            Assert.Throws<InvalidOperationException>(() => _secondLine.Reflect(point));
        }

        private static readonly object[] attachedPointReflectData =
        {
            new object[] { new Point(1, 0), new Line(new Point(2, 0), new Point(1, -1))},
        };

        //ConsumeAttach
        [TestCaseSource(nameof(attachedData))]
        public void GIVEN_Line_WHEN_the_point_is_attached_THEN_expected_true(Point attachable)
        {
            _line.ConsumeAttach(attachable);
            bool result = _line.Attached.Contains(attachable);
            Assert.That(result, Is.True);
        }

        private static readonly object[] attachedData =
        {
            new object[] {new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 1)}
        };

        [TestCaseSource(nameof(controlPointToAttachedData))]
        public void GIVEN_Line_WHEN_the_control_point_is_attached_THEN_expected_exception(Point attachable)
        {
            Assert.Throws<InvalidOperationException>(() => _line.ConsumeAttach(attachable));
        }

        private static readonly object[] controlPointToAttachedData =
        {
            new object[] {new Point(0, 0)},
            new object[] {new Point(1, 1)},
        };

        [TestCaseSource(nameof(doubleAttachedData))]
        public void GIVEN_Line_WHEN_the_attached_point_is_attached_THEN_expected_exception(Point attachable)
        {
            _line.ConsumeAttach(attachable);
            Assert.Throws<InvalidOperationException>(() => _line.ConsumeAttach(attachable));
        }

        private static readonly object[] doubleAttachedData =
        {
            new object[] {new Point(0.5f, 0.5f)}
        };

        //ConsumeDetach
        [TestCaseSource(nameof(detachData))]
        public void GIVEN_Line_WHEN_the_point_is_detached_THEN_expected_true(Point dettachable)
        {
            _line.ConsumeAttach(dettachable);

            _line.ConsumeDetach(dettachable);
            bool result = true;
            if (_line.Attached.Contains(dettachable))
                result = false;
            Assert.That(result, Is.True);
        }

        private static readonly object[] detachData =
        {
            new object[] { new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 0)},
        };
    }
}
