using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Math3D;

namespace Math3DTests
{
    [TestClass]
    public class Matrix3Tests
    {
        [TestMethod]
        public void OrthogonalTest()
        {
            Assert.IsTrue(Matrix3.Identity.IsOrthoganol);
            Assert.IsTrue(Matrix3.Identity.MultScaler(-1).IsOrthoganol);

            var orthExample = new Matrix3(0, -0.8, -0.6,
                                          0.8, -0.36, 0.48,
                                          0.6, 0.48, -0.64);
            Assert.IsTrue(orthExample.IsOrthoganol);

            var orthExample2 = new Matrix3(0, -1, 0,
                                           1, 0, 0,
                                           0, 0, -1);
            Assert.IsTrue(orthExample2.IsOrthoganol);

            var notOrthExample = new Matrix3(0, -1, 12,
                                             0, 5, 0,
                                             0, 0, -1);
            Assert.IsFalse(notOrthExample.IsOrthoganol);
        }

        [TestMethod]
        public void DeterminantTest()
        {
            const double expectedDet = -152;
            var detExample = new Matrix3(0.5, -2, 4.4,
                                         3, 0, 19,
                                         4, 0, 0);
            Assert.AreEqual(expectedDet, detExample.Determinant);
        }

        [TestMethod]
        public void InverseTest()
        {
            var invExample = new Matrix3(0.5, -2, 4.4,
                                         3, 0, 19,
                                         4, 0, 0);

            var invExpected = new Matrix3(0, 0, 0.25,
                                          -0.5, 0.11578947368421054, -0.0243421052631579,
                                          0, 0.052631578947368418, -0.039473684210526314);
            Assert.AreEqual(invExpected, invExample.InverseMatrix());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InverseSingularTest()
        {
            var zeroInv = Matrix3.Zero.InverseMatrix();
        }

        [TestMethod]
        public void TransposeTest()
        {
            var transExample = new Matrix3(0.5, -2, 4.4,
                                           3, 0, 19,
                                           4, 0, 0);

            var transExpected = new Matrix3(0.5, 3, 4,
                                            -2, 0, 0,
                                            4.4, 19, 0);
            Assert.AreEqual(transExpected, transExample.TransposeMatrix());
        }

        [TestMethod]
        public void MagnitudeTest()
        {
            Assert.AreEqual(1.0, Matrix3.Identity.Magnitude(Vector3.I));
            // TODO: add more
        }

        [TestMethod]
        public void RotationTest()
        {
            // TODO: add more
        }

        [TestMethod]
        public void ArithmaticTest()
        {
            Assert.AreEqual(2 * Matrix3.Identity, Matrix3.Identity + Matrix3.Identity);
        }
    }
}
