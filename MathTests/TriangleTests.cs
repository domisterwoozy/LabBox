using Math3D;
using Math3D.Geometry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTests.cs
{
    [TestFixture]
    public class TriangleTests
    {
        [Test]
        public void IntersectMiddleTest()
        {
            var tri = new Triangle(Vector3.J, -Vector3.I, new Vector3(1, -1, 0));
            var edge = new Edge(Vector3.K, -Vector3.K);

            Assert.That(tri.IntersectEdge(edge), Is.EquivalentTo(new[] { new CollisionInterface(Vector3.Zero, Vector3.K) }));
        }

        [Test]
        public void MissTest()
        {
            var tri = new Triangle(Vector3.J, -Vector3.I, new Vector3(1, -1, 0));
            var edge = new Edge(new Vector3(10, 10, 1), new Vector3(10, 10, -1));

            Assert.That(tri.IntersectEdge(edge), Is.EquivalentTo(Enumerable.Empty<CollisionInterface>()));
        }

        [Test]
        public void IntersectParalellTest()
        {
            var tri = new Triangle(Vector3.J, -Vector3.I, new Vector3(1, -1, 0));
            var edge = new Edge(Vector3.I, -Vector3.I);

            Assert.That(tri.IntersectEdge(edge), Is.EquivalentTo(Enumerable.Empty<CollisionInterface>()));
        }
    }
}
