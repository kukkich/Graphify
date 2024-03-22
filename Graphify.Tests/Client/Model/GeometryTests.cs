using System.Numerics;
using Graphify.Client.Model.Geometry;
using Graphify.Geometry.GeometricObjects.Curves;
using Graphify.Geometry.GeometricObjects.Points;

namespace Graphify.Tests.Client.Model
{
    internal class GeometryTests
    {
        private readonly Surface _surface = new Surface();
        private readonly Surface _emptySurface = new Surface();
        private Point _point1;
        private Point _point2;
        private Line _line;

        [SetUp]
        public void Setup()
        {
            _point1 = new Point(0, 1);
            _point2 = new Point(2, 1);
            _line = new Line(_point1, _point2);
            _surface.AddObject(_line);
        }

        //TryGetClosestObject
        [TestCaseSource(nameof(longDistance))]
        public void GIVEN_Point_WHEN_distance_between_points_is_too_long_THEN_expected_false(Vector2 point)
        {
            var result = _surface.TryGetClosestObject(point, 10);
            Assert.NotNull(result);
        }

        private static readonly object[] longDistance =
        {
            new object[] { new Vector2(2f, 1f) },
            new object[] { new Vector2(1f, 1f) },
            new object[] { new Vector2(1f, 11f) },
        };

        [TestCaseSource(nameof(lonDistance))]
        public void GIVEN_Poin_WHEN_distance_between_points_is_too_long_THEN_expected_false(Vector2 point)
        {
            var result = _surface.TryGetClosestObject(point, 10);
            Assert.IsNull(result);
        }

        private static readonly object[] lonDistance =
        {
            new object[] { new Vector2(15f, 1f) }
        };

        //TryGetClosestObject
        [TestCaseSource(nameof(loDistance))]
        public void GIVEN_Pot_WHEN_distance_between_points_is_too_long_THEN_expected_false(Vector2 point)
        {
            var result = _surface.TryGetClosestPoint(point, 10);
            Assert.NotNull(result);
        }

        private static readonly object[] loDistance =
        {
            new object[] { new Vector2(2f, 1f) },
            new object[] { new Vector2(1f, 1f) },
            new object[] { new Vector2(0f, 11f) },
        };

        [TestCaseSource(nameof(lDistance))]
        public void GIVEN_Pin_WHEN_distance_between_points_is_too_long_THEN_expected_false(Vector2 point)
        {
            var result = _surface.TryGetClosestPoint(point, 10);
            Assert.IsNull(result);
        }

        private static readonly object[] lDistance =
        {
            new object[] { new Vector2(1f, 11f) }
        };

        //AddObject
        [TestCase()]
        public void GIVEN_Po_WHEN_distance_between_points_is_too_long_THEN_expected_false()
        {
            _emptySurface.AddObject(_line);
            CollectionAssert.Contains(_emptySurface.Objects, _line);
            CollectionAssert.Contains(_emptySurface.Objects, _point1);
            CollectionAssert.Contains(_emptySurface.Objects, _point2);
        }

        [TestCase()]
        public void GIVEN_Pi_WHEN_distance_between_points_is_too_long_THEN_expected_false()
        {
            _emptySurface.AddObject(_line);
            CollectionAssert.Contains(_emptySurface.Points, _point1);
            CollectionAssert.Contains(_emptySurface.Points, _point2);
        }

        [TestCase()]
        public void GIVEN_P_WHEN_distance_between_points_is_too_long_THEN_expected_false()
        {
            _emptySurface.AddObject(_line);
            CollectionAssert.Contains(_emptySurface.Figures, _line);
        }

        [TestCase()]
        public void GIVEN__WHEN_distance_between_points_is_too_long_THEN_expected_false()
        {
            bool result = _surface.TryRemove(_line);
            Assert.IsTrue(result);
            CollectionAssert.IsEmpty(_surface.Objects);
            CollectionAssert.IsEmpty(_surface.Points);
            CollectionAssert.IsEmpty(_surface.Figures);
        }

        [TestCase()]
        public void GIVEN_n_WHEN_distance_between_points_is_too_long_THEN_expected_false()
        {
            Assert.Throws<ArgumentException>(() => _emptySurface.TryRemove(_line));
        }

        [TestCase()]
        public void GIVEN_vv_WHEN_distance_between_points_is_too_long_THEN_expected_false()
        {
            bool result = _surface.TryRemove(_point1);
            Assert.IsTrue(result);
            CollectionAssert.IsNotEmpty(_surface.Objects);
            CollectionAssert.IsNotEmpty(_surface.Points);
            CollectionAssert.DoesNotContain(_surface.Points, _point1);
            CollectionAssert.DoesNotContain(_surface.Objects, _point1);
        }

        [TestCase()]
        public void GIVEN_vev_WHEN_distance_between_points_is_too_long_THEN_expected_false()
        {
            _surface.Clear();
            CollectionAssert.IsEmpty(_surface.Objects);
            CollectionAssert.IsEmpty(_surface.Points);
            CollectionAssert.IsEmpty(_surface.Figures);
        }
    }
}
