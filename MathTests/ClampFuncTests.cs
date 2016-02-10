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
    public class ClampFuncTests
    {
        private class OutsideBoxCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { new Vector3(2, 2, 7), true };
                yield return new object[] { new Vector3(2, -3, 7), false };
                yield return new object[] { new Vector3(7, 2, 7), true };
                yield return new object[] { new Vector3(2, 2, -3), true };
                yield return new object[] { new Vector3(2, 7, -3), false };
                yield return new object[] { new Vector3(-3, 2, -3), true };
                yield return new object[] { new Vector3(2, 2, 2), false };
                yield return new object[] { new Vector3(1.51, 1.51, 2), false };
            }
        }

        [TestCaseSource(typeof(OutsideBoxCases))]
        public void OutsideBoxTest(Vector3 pos, bool outside)
        {
            Matrix3 rot = Quaternion.UnitQuaternion(Math.PI / 4, Vector3.I).ToMatrix();
            var isOutside = ClampFunctions.OutsideBox(1, 1, 10, new Vector3(2, 2, 2), rot);
            Assert.That(isOutside(pos), Is.EqualTo(outside));
        }

        private class OutsideCylinderCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { new Vector3(1, 1, 2.71), true };
                yield return new object[] { new Vector3(2, 2, 7), true };
                yield return new object[] { new Vector3(2, -3, 7), false };
                yield return new object[] { new Vector3(7, 2, 7), true };
                yield return new object[] { new Vector3(2, 2, -3), true };
                yield return new object[] { new Vector3(2, 7, -3), false };
                yield return new object[] { new Vector3(-3, 2, -3), true };
                yield return new object[] { new Vector3(2, 2, 2), false };
            }
        }

        [TestCaseSource(typeof(OutsideCylinderCases))]
        public void OutsideCylinderTest(Vector3 pos, bool outside)
        {
            var isOutside = ClampFunctions.OutsideCylinder(10, 1, new Vector3(2, 2, 2), new Vector3(0, 1, 1));
            Assert.That(isOutside(pos), Is.EqualTo(outside));
        }

        private class OutsideSphereCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { new Vector3(2, 2, 7), true };
                yield return new object[] { new Vector3(6, -4, 2), true };
                yield return new object[] { new Vector3(7, 2, 7), true };
                yield return new object[] { new Vector3(2, 2, -3), true };
                yield return new object[] { new Vector3(5.5, -5.5, -5), false };
                yield return new object[] { new Vector3(5, -5, -6), false };
                yield return new object[] { new Vector3(5, -5, -5), false };
            }
        }

        [TestCaseSource(typeof(OutsideSphereCases))]
        public void OutsideSphereTest(Vector3 pos, bool outside)
        {
            var isOutside = ClampFunctions.OutsideSphere(new Vector3(5, -5, -5), 2);
            Assert.That(isOutside(pos), Is.EqualTo(outside));
        }
    }
}
