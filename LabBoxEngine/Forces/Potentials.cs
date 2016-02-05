using Math3D.Interfaces;
using Math3D.VectorCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D
{
    public static class Potentials
    {
        private const double dummyExponent = 10; // when the coefficient is zero the exponent does not matter but still needs to exist and cannot be zero or one

        public static IScalarField InverseSquare(double strength) => CentralForce(strength, -1);
        public static IScalarField CentralForce(double strength, double power) => new SphericalScalarField(Tuple.Create(-strength, 0.0, 0.0), Tuple.Create(power, dummyExponent, dummyExponent));        
    }
}
