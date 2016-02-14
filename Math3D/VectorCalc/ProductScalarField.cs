using System;
using Util;

namespace Math3D.VectorCalc
{
    public class ProductScalarField : IScalarField
    {
        private readonly Generator<double> multiplierGen;

        public double Multiplier => multiplierGen();
        public IScalarField UnderlyingScalarField { get; }

        public ProductScalarField(IScalarField origField, double constantMultiplier) : this(origField, () => constantMultiplier) { }
        public ProductScalarField(IScalarField origField, Generator<double> multiplierGen)
        {
            this.multiplierGen = multiplierGen;
            UnderlyingScalarField = origField;
        }

        public Vector3 Gradient(Vector3 pos) => Multiplier * UnderlyingScalarField.Gradient(pos);
        public IVectorField ToVectorField() => new ProductVectorField(UnderlyingScalarField.ToVectorField(), multiplierGen);
        public double Value(Vector3 pos) => Multiplier * UnderlyingScalarField.Value(pos);
    }
}
