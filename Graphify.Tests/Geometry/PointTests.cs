using System.Numerics;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class PointTests
    {
        private Point _controlPoint = null;
        private Line _lineToAttach = null;
        private Point _point = null;
        private Point _attachedPoint = null;
        private IEqualityComparer<Point> _comparer = new PointComparer();

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
        [TestCaseSource(nameof(longDistance))]
        public void GIVEN_Point_WHEN_distance_between_points_is_too_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _point.IsNextTo(point, distance);
            Assert.IsFalse(result);
        }

        static object[] longDistance =
        {
            new object[] { new Vector2(1f, 2f), 2f },
            new object[] { new Vector2(1f, 2f), 1f }
        };

        [TestCaseSource(nameof(shortDistance))]
        public void GIVEN_Point_WHEN_distance_between_points_is_too_short_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _point.IsNextTo(point, distance);
            Assert.IsTrue(result);
        }

        static object[] shortDistance =
        {
            new object[] { new Vector2(0.5f, 0.5f), 2f },
            new object[] { new Vector2(0.5f, 0.5f), 1f }
        };

        [TestCaseSource(nameof(negativeDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _point.IsNextTo(point, distance));
        }

        static object[] negativeDataIsNextTo =
        {
            new object[] { new Vector2(0.5f, 0.5f), -1f }
        };

        //Move
        [TestCaseSource(nameof(shiftData))]
        public void GIVEN_Point_WHEN_move_THEN_expected_new_coords(Vector2 shift, Point expected)
        {
            _point.Move(shift);
            Point actual = _point;

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        static object[] shiftData =
        {
            new object[] { new Vector2(0.5f, 0.5f), new Point(0.5f,0.5f) },
            new object[] { new Vector2(-0.5f, -0.5f), new Point(-0.5f, -0.5f) }
        };

        //Rotate 
        [TestCaseSource(nameof(rotateData))]
        public void GIVEN_Point_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Point expected)
        {
            _point.Rotate(shift, angle);
            Point actual = _point;

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        static object[] rotateData =
        {
            new object[] { new Point(1, 0), 90, new Vector2(1, 1) },
            new object[] { new Point(1, 0), 540, new Vector2(2, 0) },
            new object[] { new Point(1, 0), -90, new Vector2(1, -1) }
        };

        [TestCaseSource(nameof(attachedRotateData))]
        public void GIVEN_attached_Point_WHEN_rotate_THEN_exception(Point shift, float angle)
        {
            Assert.Throws<InvalidOperationException>(() => _attachedPoint.Rotate(shift, angle));
        }

        static object[] attachedRotateData =
        {
            new object[] { new Point(1, 0), 90, new Vector2(1, 1) }
        };

        //Reflect
        [TestCaseSource(nameof(reflectData))]
        public void GIVEN_Point_WHEN_reflect_THEN_expected_new_coords(Point point, Point expected)
        {
            _point.Reflect(point);
            Point actual = _point;

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        static object[] reflectData =
        {
            new object[] { new Point(0.5f, 0.5f), new Point(1f, 1f) },
            new object[] { new Point(-0.5f, -0.5f), new Point(-1f, -1f) }
        };

        [TestCaseSource(nameof(attachedReflectData))]
        public void GIVEN_attached_Point_WHEN_reflect_THEN_exception(Point point)
        {
            Assert.Throws<InvalidOperationException>(() => _attachedPoint.Reflect(point));
        }

        static object[] attachedReflectData =
        {
            new object[] { new Point(0.5f, 0.5f) },
        };

        //CanAttachTo
        [TestCaseSource(nameof(canAttachToData))]
        public void GIVEN_attached_Point_WHEN_point_already_attached_THEN_expected_false(Line line)
        {
            bool result = _attachedPoint.CanAttachTo(line);

            Assert.IsFalse(result);
        }

        static object[] canAttachToData =
        {
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) }
        };

        [TestCase()]
        public void GIVEN_control_Point_WHEN_point_is_control_for_line_THEN_expected_false()
        {
            bool result = _controlPoint.CanAttachTo(_lineToAttach);

            Assert.IsFalse(result);
        }

        [TestCaseSource(nameof(candAttachToData))]
        public void GIVEN_Point_WHEN_can_attach_THEN_expected_true(Line line)
        {
            bool result = _point.CanAttachTo(line);

            Assert.IsTrue(result);
        }

        static object[] candAttachToData =
        {
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) }
        };

        //AttachTo
        [TestCaseSource(nameof(attachToContainsData))]
        public void GIVEN_Point_WHEN_attach_to_line_THEN_expected_line_contains_point(Line line)
        {
            _point.AttachTo(line);
            bool result = line.Attached.Contains(_point);

            Assert.IsTrue(result);
        }

        static object[] attachToContainsData =
        {
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) },
            new object[] { new Line(new Point(1, 1), new Point(-1, 1)) }
        };

        [TestCaseSource(nameof(attachToNewCoordsData))]
        public void GIVEN_Point_WHEN_attach_to_line_THEN_expected_new_coords_if_needed(Line line, Point expected)
        {
            _point.AttachTo(line);

            Assert.That(_point, Is.EqualTo(expected).Using(_comparer));
        }

        static object[] attachToNewCoordsData =
        {
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)), new Point(0, 0) },
            new object[] { new Line(new Point(1, 1), new Point(-1, 1)), new Point(0, 1) }
        };

        [TestCaseSource(nameof(attachToExceptionData))]
        public void GIVEN_Point_WHEN_point_cant_attach_THEN_expected_exception(Line line)
        {
            Assert.Throws<InvalidOperationException>(() => _attachedPoint.AttachTo(line));
        }

        static object[] attachToExceptionData =
        {
            new object[] { new Line(new Point(1, 0), new Point(-1, 0)) }
        };

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
            bool result = _lineToAttach.Attached.Contains(_point);

            Assert.IsFalse(result);
        }
    }
}
