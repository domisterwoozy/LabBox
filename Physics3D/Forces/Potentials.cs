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
        public static IScalarField InverseSquare(double strength) => ScalarFieldExtensions.SphericalField(-strength, -1);      
    }
}
