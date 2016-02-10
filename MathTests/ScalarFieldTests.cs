using Math3D;
using Math3D.VectorCalc;
using NUnit.Framework;
using System;

namespace MathTests
{
    public class ScalarFieldTests
    {
        private static IScalarField centralPotential = new SphericalScalarField(Tuple.Create(-1.0, 0.0, 0.0), Tuple.Create(-1.0, 0.0, 0.0));

        [Test]
        public void PotentialValueTest()
        {
            Vector3 point = new Vector3(5, 5, 5);

            double expectedPotentialValue = -(1.0 / point.Magnitude);
            Assert.That(expectedPotentialValue, Is.EqualTo(centralPotential.Value(point)));
        }

        [Test]
        public void PotentialForceTest()
        {
            Vector3 point = new Vector3(5, 5, 5);
            var centralForce = centralPotential.ToVectorField();

            Vector3 expectedForceValue = -(1.0 / point.MagSquared) * point.UnitDirection;
            Assert.That(expectedForceValue, Iz.EqualTo(centralForce.Value(point)).Within(Math.Pow(10, -17)));
        }
    }
}
