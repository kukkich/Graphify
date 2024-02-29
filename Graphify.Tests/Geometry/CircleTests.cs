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
    internal class CircleTests
    {
        private Circle _circle = null;
        private Circle _secondCircle = null;
        private Point _b = null;
        private IEqualityComparer<Circle> _comparer = new CircleComparer();

        [SetUp]
        public void Setup()
        {
            _circle = new Circle(new Point(0, 0), new Point(1, 1));
            _b = new Point(1, 1);
            _secondCircle = new Circle(new Point(2, 2), new Point(1, 1));
            _secondCircle.ConsumeAttach(_b);
        }

        //IsNextTo 
        [TestCaseSource(nameof(bigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _circle.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        static object[] bigDistance =
        {
            new object[] { new Vector2(3, 3), 2 },
        };

        [TestCaseSource(nameof(smallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_short_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _circle.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        static object[] smallDistance =
        {
            new object[] { new Vector2(2, 0), 1 },
        };

        [TestCaseSource(nameof(wrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _circle.IsNextTo(point, distance));
        }

        static object[] wrongDataIsNextTo =
        {
            new object[] { new Vector2(0.5f, 0.5f), -1f },
        };

        //Move 
        [TestCaseSource(nameof(moveData))]
        public void GIVEN_Circle_WHEN_move_THEN_expected_new_coords(Vector2 shift, Circle expected)
        {
            _circle.Move(shift);
            Circle actual = _circle;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));

        }

        static object[] moveData =
        {
            new object[] { new Vector2(0.5f, 0.5f), new Circle(new Point(0.5f, 0.5f), new Point(1.5f, 1.5f))},
            new object[] { new Vector2(-0.5f, -0.5f), new Circle(new Point(-0.5f, -0.5f), new Point(0.5f, 0.5f))},
            new object[] { new Vector2(0, 0), new Circle(new Point(0, 0), new Point(1, 1))},
        };

        [TestCaseSource(nameof(attachedPointMoveData))]
        public void GIVEN_Circle_WHEN_move_attached_point_THEN_expected_exception(Vector2 shift, Circle expected)
        {
            Assert.Throws<InvalidOperationException>(() => _circle.Move(shift));
        }

        static object[] attachedPointMoveData =
        {
            new object[] { new Vector2(-0.5f, -0.5f), new Circle(new Point(-0.5f, -0.5f), new Point(0.5f, 0.5f))},
        };

        //Rotate 
        //функция не работает 
        [TestCaseSource(nameof(rotateData))]
        public void GIVEN_Circle_WHEN_rotate_THEN_expected_new_coords(Point shift, float angle, Circle expected)
        {
            _circle.Rotate(shift, angle);
            Circle actual = _circle;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        static object[] rotateData =
        {
            new object[] { new Point(1, 0), 90, new Circle(new Point(1, 1), new Point(2, 0))},
            new object[] { new Point(1, 0), 180, new Circle(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), -90, new Circle(new Point(0, 0), new Point(-1, 1))},
        };

        [TestCaseSource(nameof(attachedPointRotateData))]
        public void GIVEN_Circle_WHEN_rotate_attached_point_THEN_expected_exception(Point shift, float angle, Circle expected)
        {
            Assert.Throws<InvalidOperationException>(() => _circle.Rotate(shift, angle));
        }

        static object[] attachedPointRotateData =
        {
            new object[] { new Point(1, 0), 90, new Circle(new Point(1, 1), new Point(2, 0))},
        };

        //Reflect 
        //функция которая делает Rotate на 180 градусов  
        //не нужно ли нам сделать отражение относительно прямой вместо этого 
        [TestCaseSource(nameof(reflectData))]
        public void GIVEN_Circle_WHEN_reflect_THEN_expected_new_coords(Point point, Circle expected)
        {
            _circle.Reflect(point);
            Circle actual = _circle;
            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        static object[] reflectData =
        {
            new object[] { new Point(1, 0), new Circle(new Point(2, 0), new Point(1, -1))},
            new object[] { new Point(0, 0), new Circle(new Point(0, 0), new Point(-1, -1))},
            new object[] { new Point(1, 2), new Circle(new Point(2, 4), new Point(1, 3))}
        };

        [TestCaseSource(nameof(attachedPointReflectData))]
        public void GIVEN_Circle_WHEN_reflect_attached_point_THEN_expected_exception(Point point, Circle expected)
        {
            Assert.Throws<InvalidOperationException>(() => _circle.Reflect(point));
        }

        static object[] attachedPointReflectData =
        {
            new object[] { new Point(1, 0), new Circle(new Point(2, 0), new Point(1, -1))},
        };

        //ConsumeAttach 
        [TestCaseSource(nameof(attachedData))]
        public void GIVEN_Circle_WHEN_the_point_is_attached_THEN_expected_true(Point attachable)
        {
            _circle.ConsumeAttach(attachable);
            bool result = _circle.Attached.Contains(attachable);
            Assert.That(result, Is.True);
        }

        static object[] attachedData =
       {
            new object[] {new Point(-1, -1)},
            new object[] {new Point(0, 2)}
        };

        [TestCaseSource(nameof(controlPointToAttachedData))]
        public void GIVEN_Circle_WHEN_the_control_point_is_attached_THEN_expected_exception(Point attachable)
        {
            Assert.Throws<InvalidOperationException>(() => _circle.ConsumeAttach(attachable));

        }

        static object[] controlPointToAttachedData =
        {
            new object[] {new Point(0, 0)},
        };

        [TestCaseSource(nameof(doubleAttachedData))]
        public void GIVEN_Circle_WHEN_the_attached_point_is_attached_THEN_expected_exception(Point attachable)
        {
            _circle.ConsumeAttach(attachable);
            Assert.Throws<InvalidOperationException>(() => _circle.ConsumeAttach(attachable));
        }

        static object[] doubleAttachedData =
        {
             new object[] {new Point(3.5f, 3.5f)}
        };


        //ConsumeDetach 
        [TestCaseSource(nameof(detachData))]
        public void GIVEN_Circle_WHEN_the_point_is_detached_THEN_expected_true(Point dettachable)
        {
            _circle.ConsumeAttach(dettachable);
            _circle.ConsumeDetach(dettachable);

            bool result = _circle.Attached.Contains(dettachable);
            
            Assert.That(result, Is.True);
        }

        static object[] detachData =
        {
            new object[] { new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 0)},
        };
    }
}
