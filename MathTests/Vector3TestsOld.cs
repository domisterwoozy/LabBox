using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Math3D;

namespace MathTests
{
    [TestClass]
    public class Vector3TestsOld
    {
        private static readonly Random rand = new Random();

        private static Vector3 RandVector() => new Vector3(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());

        [TestMethod]
        public static void VectorEqual(Vector3 a, Vector3 b, double componentTolerance)
        {
            Assert.AreEqual(a.X, b.X, componentTolerance);
            Assert.AreEqual(a.Y, b.Y, componentTolerance);
            Assert.AreEqual(a.Z, b.Z, componentTolerance);
        }

        [TestMethod]
        public void RandomInverseVector()
        {
            var initialVector = RandVector();
            var expected = new Vector3(-initialVector.X, -initialVector.Y, -initialVector.Z);

            var actual = initialVector.Inverse;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ZeroInverseVector()
        {
            Assert.AreEqual(Vector3.Zero, Vector3.Zero.Inverse);
        }

        [TestMethod]
        public void ZeroMagnitude()
        {
            Assert.AreEqual(Vector3.Zero.Magnitude, 0.0);
        }

        [TestMethod]
        public void RandomMagMagSquared()
        {
            Vector3 randomVector = RandVector();

            Assert.AreEqual(randomVector.Magnitude * randomVector.Magnitude, randomVector.MagSquared, MathConstants.DoubleEqualityTolerance);
        }

        [TestMethod]
        public void NormalVectorMagnitude()
        {
            Vector3 randomVector = RandVector();

            Assert.AreEqual(randomVector.UnitDirection.Magnitude, 1.0, MathConstants.DoubleEqualityTolerance);
        }

        [TestMethod]
        public void CrossUnitVectors()
        {
            double randA = rand.NextDouble();
            double randB = rand.NextDouble();

            Assert.AreEqual((randA * Vector3.I) % (randB * Vector3.J), (randA * randB) * Vector3.K);
        }

        [TestMethod]
        public void VectorArithmatic()
        {
            double randA = rand.NextDouble();
            double randB = rand.NextDouble();

            Assert.AreEqual((randA * Vector3.I) % (randB * Vector3.J), (randA * randB) * Vector3.K);
        }

        [TestMethod]
        public void DotProduct()
        {
            Vector3 randVect = RandVector();
            Assert.AreEqual(randVect.MagSquared, randVect * randVect);
        }
    }
}
