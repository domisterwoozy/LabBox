﻿using Math3D;
using Math3D.Interfaces;
using Math3D.VectorCalc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTests
{
    public class ScalarFieldTests
    {
        private static IScalarField centralPotential = new SphericalScalarField(Tuple.Create(-1.0, 0.0, 0.0), Tuple.Create(-1.0, 0.0, 0.0));

        public void PotentialValueTest()
        {
            Vector3 point = new Vector3(5, 5, 5);

            double expectedPotentialValue = -(1.0 / point.Magnitude);
            Assert.That(expectedPotentialValue, Is.EqualTo(centralPotential.Value(point)));
        }

        public void PotentialForceTest()
        {
            Vector3 point = new Vector3(5, 5, 5);
            var centralForce = centralPotential.ToVectorField();

            Vector3 expectedForceValue = -(1.0 / point.MagSquared) * point.UnitDirection;
            Assert.That(expectedForceValue, Is.EqualTo(centralForce.Value(point)));
        }
    }
}