using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class PointTests
    {
        private Point _point = null;
        private IEqualityComparer<Point> _comparer = new PointComparer();

        [SetUp]
        public void Setup()
        {
            _point = new Point(0, 0);
        }

        //IsNextTo
        [TestCaseSource(nameof(bigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _point.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        [TestCaseSource(nameof(smallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _point.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        [TestCaseSource(nameof(wrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _point.IsNextTo(point, distance));
        }

        static object[] bigDistance =
        {
            new object[] { new Vector2(1f, 2f), 2f },
            new object[] { new Vector2(1f, 2f), 1f }
        };

        static object[] smallDistance =
        {
            new object[] { new Vector2(0.5f, 0.5f), 2f },
            new object[] { new Vector2(0.5f, 0.5f), 1f }
        };

        static object[] wrongDataIsNextTo =
        {
            new object[] { new Vector2(0.5f, 0.5f), -1f },
            new object[] { new Vector2(0.5f, 0.5f), 0f }
        };

        //Move
        [TestCaseSource(nameof(shiftData))]
        public void GIVEN_Point_WHEN_move_THEN_expected_new_coords(Vector2 shift, Vector2 expected)
        {
            _point.Move(shift);
            Vector2 actual = new Vector2(_point.X, _point.Y);
            Point point = new Point(_point.X,_point.Y);

            Assert.That(_point, Is.EqualTo(point).Using(_comparer));
        }

        static object[] shiftData =
        {
            new object[] { new Vector2(0.5f, 0.5f), new Vector2(0.5f,0.5f)},
            new object[] { new Vector2(-0.5f, -0.5f), new Vector2(-0.5f, -0.5f) }
        };

        //Rotate
        [TestCaseSource(nameof(rotateData))]
        public void GIVEN_Point_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Vector2 expected)
        {
            _point.Rotate(shift, angle);
            Vector2 actual = new Vector2(_point.X, _point.Y);

            Assert.AreEqual(expected, actual);
        }

        static object[] rotateData =
        {
            new object[] { new Point(1, 0), 90, new Vector2(1, 1) },
            new object[] { new Point(1, 0), 540, new Vector2(2, 0) },
            new object[] { new Point(1, 0), -90, new Vector2(1, -1) }
        };

    }
}
