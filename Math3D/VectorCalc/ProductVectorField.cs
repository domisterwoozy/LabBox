using System;

namespace Math3D.VectorCalc
{
    public class ProductVectorField : IVectorField
    {
        public double Multiplier { get; }
        public IVectorField UnderlyingVectorField { get; }

        public ProductVectorField(double multiplier, IVectorField vectorField)
        {
            Multiplier = multiplier;
            UnderlyingVectorField = vectorField;
        }

        public Vector3 Curl(Vector3 pos) => Multiplier * UnderlyingVectorField.Curl(pos);

        public double Divergence(Vector3 pos) => Multiplier * UnderlyingVectorField.Divergence(pos);

        public Vector3 Value(Vector3 pos) => Multiplier * UnderlyingVectorField.Value(pos);
    }
}
