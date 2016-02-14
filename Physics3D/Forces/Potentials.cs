﻿using Math3D.VectorCalc;

namespace Physics3D
{
    public static class Potentials
    {
        public static IScalarField InverseSquare(double strength) => FieldExtensions.SphericalScalarField(-strength, -1);      

    }
}
