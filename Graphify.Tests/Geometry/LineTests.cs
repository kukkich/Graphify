using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Graphify.Geometry.GeometricObjects.Interfaces;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NUnit.Framework.Constraints;

namespace Graphify.Tests.Geometry
{
    internal class LineTests
    {
        private Line _line = null;
        private Point _a = new Point(0, 0);
        private Point _b = new Point(1, 1);
        private Point dettachable = new Point(0.5f, 0.5f);

        [SetUp]
        public void Setup()
        {
            _line = new Line(new Point(0, 0), new Point(1, 1));
        }

        //IsNextTo
        [TestCaseSource(nameof(bigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _line.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        [TestCaseSource(nameof(smallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _line.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        [TestCaseSource(nameof(wrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _line.IsNextTo(point, distance));
        }

        static object[] bigDistance =
        {
            new object[] { new Vector2(1.5f, -0.5f), 1f },
        };

        static object[] smallDistance =
        {
            new object[] { new Vector2(1, 0), 1 },
        };

        static object[] wrongDataIsNextTo =
        {
            new object[] { new Vector2(0.5f, 0.5f), -1f },
        };

        //Move
        [TestCaseSource(nameof(moveData))]
        public void GIVEN_Line_WHEN_move_THEN_expected_new_coords(Vector2 shift, Line expected)
        {
            _line.Move(shift);
            Line actual = _line;
            Assert.AreEqual(expected, actual);
        }

        static object[] moveData =
        {
            new object[] { new Vector2(0.5f, 0.5f), new Line(new Point(0.5f, 0.5f), new Point(1.5f, 1.5f))},
            new object[] { new Vector2(-0.5f, -0.5f), new Line(new Point(-0.5f, -0.5f), new Point(0.5f, 0.5f))},
            new object[] { new Vector2(0, 0), new Line(new Point(0, 0), new Point(1, 1))},
        };

        //Rotate
        //функция не работает
        [TestCaseSource(nameof(rotateData))]
        public void GIVEN_Line_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Line expected)
        {
            _line.Rotate(shift, angle);
            Line actual = _line;
            Assert.AreEqual(expected, actual);
        }

        static object[] rotateData =
        {
            new object[] { new Point(1, 0), 90, new Line(new Point(1, 1), new Point(2, 0))},
            new object[] { new Point(1, 0), 180, new Line(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), -90, new Line(new Point(0, 0), new Point(-1, 1))},
        };

        //Reflect
        //функция которая делает Rotate на 180 градусов 
        //не нужно ли нам сделать отражение относительно прямой вместо этого
        [TestCaseSource(nameof(reflectData))]
        public void GIVEN_Line_WHEN_reflect_THEN_expected_new_coords(Point point, Line expected)
        {
            _line.Reflect(point);
            Line actual = _line;
            Assert.AreEqual(expected, actual);
        }

        static object[] reflectData =
        {
            new object[] { new Point(1, 0), new Line(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), new Line(new Point(0, 0), new Point(-1, -1))},
            new object[] { new Point(1, 2), new Line(new Point(2, 4), new Point(1, 3))}
        };

        //ConsumeAttach
        [TestCaseSource(nameof(attachedData))]
        public void GIVEN_Line_WHEN_the_point_is_attached_THEN_expected_true(Point attachable)
        {
            _line.ConsumeAttach(attachable);
            bool result = false;
            if(_line.Attached.Contains(attachable))
                result = true;
            Assert.That(result, Is.True);
        }

        [TestCaseSource(nameof(notAttachedData))]
        public void GIVEN_Line_WHEN_the_point_is_not_attached_THEN_expected_false(Point attachable)
        {
            _line.ConsumeAttach(attachable);
            bool result = false;
            if (_line.Attached.Contains(attachable))
                result = true;
            Assert.That(result, Is.False);
        }

        static object[] attachedData =
        {
            new object[] {new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 1)}
        };

        static object[] notAttachedData =
        {
            new object[] {new Point(0, 0)},
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

        static object[] detachData =
        {
            new object[] { new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 0)},
        };
    }
}
