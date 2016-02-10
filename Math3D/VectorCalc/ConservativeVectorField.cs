﻿namespace Math3D.VectorCalc
{
    public abstract class ConservativeVectorField : IVectorField
    {
        /// <summary>
        /// The curl of a conservative vector field is always zero no matter the input.
        /// </summary>
        public Vector3 Curl(Vector3 pos) => Vector3.Zero;

        public abstract double Divergence(Vector3 pos);
        public abstract Vector3 Value(Vector3 pos);
    }
}
