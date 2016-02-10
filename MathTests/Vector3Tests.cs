using Math3D;
using NUnit.Framework;
using System;
using System.Collections;

namespace MathTests
{
    public class Vector3Tests
    {
        [DatapointSource]
        public Vector3[] values = new[]
        {
            Vector3.Zero,
            Vector3.I, Vector3.J, Vector3.K,
            Vector3.I + Vector3.J,
            -1 * Vector3.K + Vector3.I,
            new Vector3(-1.5, 9.9, 0.1),
            new Vector3(-0.5, 0.5, 0),
            new Vector3(-0.5, -1.5, -0.1)
        };

        #region Addition/Subtraction  
        private class AdditionCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new[] { Vector3.Zero, Vector3.Zero, Vector3.Zero };
                yield return new[] { Vector3.I, Vector3.Zero, Vector3.I };
            }
        }

        [TestCaseSource(typeof(AdditionCases))]
        public void AdditionTest(Vector3 a, Vector3 b, Vector3 sum)
        {
            Assert.That(a + b, Is.EqualTo(sum));
        }  

        [Test]
        public void RandomAdditionTest([RandomVector3(3)]Vector3 a, [RandomVector3(3)]Vector3 b)
        {
            var sum = new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            AdditionTest(a, b, sum);
        }        

        [Theory]
        public void AdditiveIdentityDefinition(Vector3 a)
        {
            AdditionTest(a, Vector3.Zero, a);
        }

        [Theory]
        public void SubractionDefinition(Vector3 a, Vector3 b)
        {
            Assert.That(a + -b, Is.EqualTo(a - b));
            Assert.That(b + -a, Is.EqualTo(b - a));
        }
        #endregion

        #region Cross Product
        [TestCaseSource(typeof(CrossProductCases))]
        public void CrossProductTest(Vector3 a, Vector3 b, Vector3 crossProduct)
        {
            Assert.That(a ^ b, Is.EqualTo(crossProduct));
        }

        private class CrossProductCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new[] { Vector3.Zero, Vector3.Zero, Vector3.Zero };
                yield return new[] { Vector3.I, Vector3.Zero, Vector3.Zero };
                yield return new[] { Vector3.I, Vector3.J, Vector3.K };
                yield return new[] { -Vector3.I, Vector3.J, -Vector3.K };
            }
        }

        [Theory]
        public void CrossProductPerpindicularTest(Vector3 a, Vector3 b)
        {
            Vector3 res = a ^ b;
            Assert.That(res * a, Is.EqualTo(0).Within(Math.Pow(10, -10)));
            Assert.That(res * b, Is.EqualTo(0).Within(Math.Pow(10, -10)));
        }
        #endregion

        #region Dot Product
        [TestCaseSource(typeof(DotProductCases))]
        public void DotProductTest(Vector3 a, Vector3 b, double dotProduct)
        {
            Assert.That(a * b, Is.EqualTo(dotProduct));
        }

        private class DotProductCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new ValueType[] { Vector3.Zero, Vector3.Zero, 0 };
                yield return new ValueType[] { Vector3.I, Vector3.Zero, 0 };
                yield return new ValueType[] { Vector3.I, Vector3.J, 0 };
                yield return new ValueType[] { Vector3.I, Vector3.I, 1 };
                yield return new ValueType[] { new Vector3(-0.5, 2.0, 1.0), new Vector3(5.0, 1.0, 0.5), 0 };
                yield return new ValueType[] { new Vector3(-0.5, 2.0, 1.0), new Vector3(5.0, 1.0, 0), -0.5 };
            }
        }
        #endregion


    }
}
