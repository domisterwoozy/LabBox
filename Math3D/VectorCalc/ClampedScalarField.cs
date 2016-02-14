using Math3D.Geometry;
using System;

namespace Math3D.VectorCalc
{
    public class ClampedScalarField : IScalarField
    {
        /// <summary>
        /// The underlying scalar field that is being clamped.
        /// </summary>
        public IScalarField UnderlyingScalarField { get; }

        /// <summary>
        /// A function that determines if a position in 3D space is being clamped. At a clamped location the value of this scalar field is zero.
        /// </summary>
        public Func<Vector3, bool> ClampFunction { get; }

        public ClampedScalarField(IScalarField origScalarField, Func<Vector3, bool> clampFunction)
        {
            UnderlyingScalarField = origScalarField;
            ClampFunction = clampFunction;
        }

        public double Value(Vector3 pos)
        {
            if (ClampFunction(pos)) return 0;
            return UnderlyingScalarField.Value(pos);
        }

        public Vector3 Gradient(Vector3 pos)
        {
            if (ClampFunction(pos)) return Vector3.Zero;
            return UnderlyingScalarField.Gradient(pos);
        }               

        public IVectorField ToVectorField()
        {
            return new ClampedVectorField(UnderlyingScalarField.ToVectorField(), ClampFunction);
        }
    }
}
