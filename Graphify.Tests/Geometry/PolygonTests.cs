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
        public void GIVEN_Polygon_WHEN_move_THEN_expected_new_coords(Vector2 shift, Polygon expected)
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
        public void GIVEN_Polygon_WHEN_move_attached_point_THEN_expected_exception(Vector2 shift)
        {
            Assert.Throws<InvalidOperationException>(() => _secondPolygon.Move(shift));
        }

        private static readonly object[] attachedPointMoveData =
        {
            new object[] { new Vector2(1f, 1f)},
        };


        //ConsumeAttach
        [TestCaseSource(nameof(attachedData))]
        public void GIVEN_Polygon_WHEN_the_point_is_attached_THEN_expected_true(Point attachable)
        {
            _polygon.ConsumeAttach(attachable);
            bool result = _polygon.Attached.Contains(attachable);
            Assert.That(result, Is.True);
        }

        private static readonly object[] attachedData =
        {
            new object[] {new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 1)}
        };

        [TestCaseSource(nameof(controlPointToAttachedData))]
        public void GIVEN_Polygon_WHEN_the_control_point_is_attached_THEN_expected_exception(Point attachable)
        {
            Assert.Throws<InvalidOperationException>(() => _polygon.ConsumeAttach(attachable));
        }

        private static readonly object[] controlPointToAttachedData =
        {
        };

        [TestCaseSource(nameof(doubleAttachedData))]
        public void GIVEN_Polygon_WHEN_the_attached_point_is_attached_THEN_expected_exception(Point attachable)
        {
            _polygon.ConsumeAttach(attachable);
            Assert.Throws<InvalidOperationException>(() => _polygon.ConsumeAttach(attachable));
        }

        private static readonly object[] doubleAttachedData =
        {
            new object[] {new Point(0.5f, 0.5f)}
        };

        //ConsumeDetach
        [TestCaseSource(nameof(detachData))]
        public void GIVEN_Polygon_WHEN_the_point_is_detached_THEN_expected_true(Point dettachable)
        {
            _polygon.ConsumeAttach(dettachable);

            _polygon.ConsumeDetach(dettachable);
            bool result = true;
            if (_polygon.Attached.Contains(dettachable))
                result = false;
            Assert.That(result, Is.True);
        }

        private static readonly object[] detachData =
        {
            new object[] { new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 0)},
        };


        //Rotate
        //аналогично move
        [TestCaseSource(nameof(rotateData))]
        public void GIVEN_Polygon_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Polygon expected)
        {
            _polygon.Rotate(shift, angle);
            Polygon actual = _polygon;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] rotateData =
        {
            new object[] { new Point(1, 1), 90, new Polygon([new Point(0, 2), new Point(1, 2), new Point(1, 1), new Point(0, 1), new Point(0, 2)]) },
        };

        [TestCaseSource(nameof(attachedPointRotateData))]
        public void GIVEN_Polygon_WHEN_rotate_attached_point_THEN_expected_exception(Point shift, float angle)
        {
            Assert.Throws<InvalidOperationException>(() => _secondPolygon.Rotate(shift, angle));
        }

        private static readonly object[] attachedPointRotateData =
        {
            new object[] { new Point(1, 0), 90},
        };

        //Reflect
        [TestCaseSource(nameof(reflectData))]
        public void GIVEN_Polygon_WHEN_reflect_THEN_expected_new_coords(Point point, Polygon expected)
        {
            _polygon.Reflect(point);
            Polygon actual = _polygon;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        private static readonly object[] reflectData =
        {
            new object[] { new Point(2, 2), new Polygon([new Point(4, 4), new Point(4, 3), new Point(3, 3), new Point(3, 4), new Point(4, 4)]) },
        };

        [TestCaseSource(nameof(attachedPointReflectData))]
        public void GIVEN_Polygon_WHEN_reflect_attached_point_THEN_expected_exception(Point point)
        {
            Assert.Throws<InvalidOperationException>(() => _secondPolygon.Reflect(point));
        }

        private static readonly object[] attachedPointReflectData =
        {
            new object[] { new Point(1, 0)},
        };
    }
}
