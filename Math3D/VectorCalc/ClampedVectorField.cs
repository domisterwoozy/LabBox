using System;

namespace Math3D.VectorCalc
{
    public class ClampedVectorField : IVectorField
    {        
        public IVectorField UnderlyingVectorField { get; }
        public Func<Vector3, bool> ClampFunction;

        public ClampedVectorField(IVectorField vectorField, Func<Vector3, bool> clampFunction)
        {
            UnderlyingVectorField = vectorField;
            ClampFunction = clampFunction;
        }

        public Vector3 Value(Vector3 pos)
        {
            if (ClampFunction(pos)) return Vector3.Zero;
            return UnderlyingVectorField.Value(pos);
        }
    }
}
