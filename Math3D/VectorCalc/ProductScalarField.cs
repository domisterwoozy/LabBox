using System;

namespace Math3D.VectorCalc
{
    public class ProductScalarField : AbstractScalarField
    {
        public double Multiplier { get; }

        public IScalarField UnderlyingScalarField { get; }

        public ProductScalarField(double multiplier, IScalarField origField)
        {
            Multiplier = multiplier;
            UnderlyingScalarField = origField;
        }

        public override Vector3 Gradient(Vector3 pos) => Multiplier * UnderlyingScalarField.Gradient(pos);

        public override IVectorField ToVectorField() => new ProductVectorField(Multiplier, UnderlyingScalarField.ToVectorField());

        public override double Value(Vector3 pos) => Multiplier * UnderlyingScalarField.Value(pos);
    }
}
