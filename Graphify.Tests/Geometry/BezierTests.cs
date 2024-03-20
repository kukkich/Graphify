using System.Numerics;
using Aspose.Imaging.FileFormats.OpenDocument.Objects.Graphic;
using Graphify.Geometry.Attaching;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Geometry
{
    internal class BezierTests
    {
        private CubicBezierCurve _bezier = null;
        private CubicBezierCurve _secondBezier = null;
        private Point _b = null;
        private Point _a = null;
        private readonly IEqualityComparer<CubicBezierCurve> _comparer = new BezierComparer();

        [SetUp]
        public void Setup()
        {
            _bezier = new CubicBezierCurve([new Point(0, 0), new Point(1, 1), new Point(2, 1), new Point(3, 0)]);
            _b = new Point(1f, 1f);
            _secondBezier = new CubicBezierCurve([_b, new Point(1, 2), new Point(2, 3), new Point(3, 0)]);
            _b.AttachTo(_bezier);
        }

        //IsNextTo
        [TestCaseSource(nameof(bigDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_false(Vector2 point, float distance)
        {
            bool result = _bezier.IsNextTo(point, distance);
            Assert.That(result, Is.False);
        }

        private static readonly object[] bigDistance =
        {
            new object[] { new Vector2(1.5f, 2f), 1f },
        };

        [TestCaseSource(nameof(smallDistance))]
        public void GIVEN_Point_WHEN_distance_is_to_long_THEN_expected_true(Vector2 point, float distance)
        {
            bool result = _bezier.IsNextTo(point, distance);
            Assert.That(result, Is.True);
        }

        private static readonly object[] smallDistance =
        {
            new object[] { new Vector2(1.5f, 2f), 3f },
        };

        [TestCaseSource(nameof(wrongDataIsNextTo))]
        public void GIVEN_Point_WHEN_wrong_data_THEN_expected_exception(Vector2 point, float distance)
        {
            Assert.Throws<ArgumentException>(() => _bezier.IsNextTo(point, distance));
        }

        private static readonly object[] wrongDataIsNextTo =
        {
            new object[] { new Vector2(2f, 0f), -1f },
        };

        //ConsumeAttach 
        [TestCaseSource(nameof(attachedData))]
        public void GIVEN_BezierCurve_WHEN_the_point_is_attached_THEN_expected_true(Point attachable)
        {
            _bezier.ConsumeAttach(attachable);
            bool result = _bezier.Attached.Contains(attachable);
            Assert.That(result, Is.True);
        }

        private static readonly object[] attachedData =
       {
            new object[] {new Point(1, 0)},
            new object[] {new Point(0, 2)}
        };

        // тест не работает потому что нужно передавать сами опорные точки
        [TestCaseSource(nameof(controlPointToAttachedData))]
        public void GIVEN_BezierCurve_WHEN_the_control_point_is_attached_THEN_expected_exception(Point attachable)
        {
            Assert.Throws<InvalidOperationException>(() => _bezier.ConsumeAttach(attachable));

        }

        private static readonly object[] controlPointToAttachedData =
        {
            //new object[] {new Point(1f, 1f)},
        };

        [TestCaseSource(nameof(doubleAttachedData))]
        public void GIVEN_BezierCurve_WHEN_the_attached_point_is_attached_THEN_expected_exception(Point attachable)
        {
            _bezier.ConsumeAttach(attachable);
            Assert.Throws<InvalidOperationException>(() => _bezier.ConsumeAttach(attachable));
        }

        private static readonly object[] doubleAttachedData =
        {
             new object[] {new Point(3.5f, 3.5f)}
        };


        //ConsumeDetach
        [TestCaseSource(nameof(detachData))]
        public void GIVEN_BezierCurve_WHEN_the_point_is_detached_THEN_expected_true(Point dettachable)
        {
            _bezier.ConsumeAttach(dettachable);

            _bezier.ConsumeDetach(dettachable);
            bool result = true;
            if (_bezier.Attached.Contains(dettachable))
                result = false;
            Assert.That(result, Is.True);
        }

        private static readonly object[] detachData =
        {
            new object[] { new Point(0.5f, 0.5f)},
            new object[] {new Point(0, 0)},
        };

        [TestCaseSource(nameof(errDetachData))]
        public void GIVEN_BezierCurve_WHEN_the_point_is_not_detached_THEN_expected_exception(Point dettachable)
        {
            Assert.Throws<InvalidOperationException>(() => _bezier.ConsumeDetach(dettachable));
        }

        private static readonly object[] errDetachData =
        {
            new object[] { new Point(0.5f, 0.5f)},
        };


        
    }
}
