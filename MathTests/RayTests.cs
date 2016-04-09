using Math3D;
using Math3D.Geometry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;

namespace MathTests.cs
{
    [TestFixture]
    public class RayTests
    {
        [Test]
        public void EmptyTest()
        {
            Assert.IsNull(Ray.Cast(Enumerable.Empty<TransformedObj<IEdgeIntersector>>(), Vector3.Zero, Vector3.I, 100));
        }

        [Test]
        public void NoDirTest()
        {
            Assert.Throws<ArgumentException>(() => Ray.Cast(Enumerable.Empty<TransformedObj<IEdgeIntersector>>(), Vector3.Zero, Vector3.Zero, 100));
        }

        [Test]
        public void MultiObjectTest()
        {
            var closeIntersector = A.Fake<IEdgeIntersector>();
            var farIntersector = A.Fake<IEdgeIntersector>();

            var closeTrans = new Transform(Vector3.I, Matrix3.Identity);
            var farTrans = new Transform(5 * Vector3.I, Matrix3.Identity);

            var closeObj = new TransformedObj<IEdgeIntersector>(closeTrans, closeIntersector);
            var farObj = new TransformedObj<IEdgeIntersector>(farTrans, farIntersector);            

            A.CallTo(() => closeIntersector.FindIntersections(default(Edge)))
                .WithAnyArguments()
                .Returns(new[] { new Intersection() });
            A.CallTo(() => farIntersector.FindIntersections(default(Edge)))
                .WithAnyArguments()
                .Returns(new[] { new Intersection() });

            Assert.That(Ray.Cast(new[] { farObj, closeObj }, Vector3.Zero, Vector3.I, 100).Value.HitObject, Is.EqualTo(closeObj));
        }

        [Test]
        public void MultiObjectTestTwo()
        {
            var closeIntersector = A.Fake<IEdgeIntersector>();
            var farIntersector = A.Fake<IEdgeIntersector>();

            var closeTrans = new Transform(Vector3.I, Matrix3.Identity);
            var farTrans = new Transform(5 * Vector3.I, Matrix3.Identity);

            var closeObj = new TransformedObj<IEdgeIntersector>(closeTrans, closeIntersector);
            var farObj = new TransformedObj<IEdgeIntersector>(farTrans, farIntersector);

            A.CallTo(() => closeIntersector.FindIntersections(default(Edge)))
                .WithAnyArguments()
                .Returns(new Intersection[] { });
            A.CallTo(() => farIntersector.FindIntersections(default(Edge)))
                .WithAnyArguments()
                .Returns(new[] { new Intersection() });

            Assert.That(Ray.Cast(new[] { farObj, closeObj }, Vector3.Zero, Vector3.I, 100).Value.HitObject, Is.EqualTo(farObj));
        }

        [Test]
        public void MultiObjectTestThree()
        {
            var closeIntersector = A.Fake<IEdgeIntersector>();
            var farIntersector = A.Fake<IEdgeIntersector>();

            var closeTrans = new Transform(Vector3.I, Matrix3.Identity);
            var farTrans = new Transform(5 * Vector3.I, Matrix3.Identity);

            var closeObj = new TransformedObj<IEdgeIntersector>(closeTrans, closeIntersector);
            var farObj = new TransformedObj<IEdgeIntersector>(farTrans, farIntersector);

            A.CallTo(() => closeIntersector.FindIntersections(default(Edge)))
                .WithAnyArguments()
                .Returns(new Intersection[] { });
            A.CallTo(() => farIntersector.FindIntersections(default(Edge)))
                .WithAnyArguments()
                .Returns(new Intersection[] { });

            Assert.IsNull(Ray.Cast(new[] { farObj, closeObj }, Vector3.Zero, Vector3.I, 100));
        }


    }
}
