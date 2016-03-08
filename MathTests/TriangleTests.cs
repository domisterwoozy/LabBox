using Math3D;
using Math3D.Geometry;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTests.cs
{
    [TestFixture]
    public class TriangleTests
    {
        private class EdgeIntersectCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var t1 = new TriangleIntersector(Vector3.I, Vector3.J, Vector3.K);
                var t1n = new Vector3(1, 1, 1).UnitDirection;
                var e1 = new Edge(Vector3.Zero, new Vector3(1, 1, 1));
                var e2 = new Edge(Vector3.Zero, 2 * Vector3.I);
                var e3 = new Edge(Vector3.Zero, new Vector3(-1, -1, -1));
                var e4 = new Edge(Vector3.I, Vector3.J);
                var i1 = new Intersection(new Vector3(1.0/3.0, 1.0/3.0, 1.0/3.0), t1n);
                var i2 = new Intersection(Vector3.I, t1n);
                var none = Enumerable.Empty<Intersection>();

                yield return new object[] { t1, e1, new[] { i1 } };
                yield return new object[] { t1, e2, new[] { i2 } };
                yield return new object[] { t1, e3, none };
                yield return new object[] { t1, e4, none };
            }
        }

        [TestCaseSource(typeof(EdgeIntersectCases))]
        public void EdgeIntersectTest(TriangleIntersector t, Edge e, IEnumerable<Intersection> intersects)
        {
            Assert.That(t.FindIntersections(e), Is.EquivalentTo(intersects));
        }


        [Test]
        public void IntersectMiddleTest()
        {
            var tri = new TriangleIntersector(Vector3.J, -Vector3.I, new Vector3(1, -1, 0));
            var edge = new Edge(Vector3.K, -Vector3.K);

            Assert.That(tri.FindIntersections(edge), Is.EquivalentTo(new[] { new Intersection(Vector3.Zero, Vector3.K) }));
        }

        [Test]
        public void MissTest()
        {
            var tri = new TriangleIntersector(Vector3.J, -Vector3.I, new Vector3(1, -1, 0));
            var edge = new Edge(new Vector3(10, 10, 1), new Vector3(10, 10, -1));

            Assert.That(tri.FindIntersections(edge), Is.EquivalentTo(Enumerable.Empty<Intersection>()));
        }

        [Test]
        public void IntersectParalellTest()
        {
            var tri = new TriangleIntersector(Vector3.J, -Vector3.I, new Vector3(1, -1, 0));
            var edge = new Edge(Vector3.I, -Vector3.I);

            Assert.That(tri.FindIntersections(edge), Is.EquivalentTo(Enumerable.Empty<Intersection>()));
        }
    }
}
