using System;
using Util;

namespace Math3D.VectorCalc
{
    public class ProductVectorField : IVectorField
    {
        private readonly Generator<double> multiplierGen;

        public double Multiplier => multiplierGen();
        public IVectorField UnderlyingVectorField { get; }

        public ProductVectorField(IVectorField origField, double constantMultiplier) : this(origField, () => constantMultiplier) { }
        public ProductVectorField(IVectorField vectorField, Generator<double> multiplierGen)
        {
            this.multiplierGen = multiplierGen;
            UnderlyingVectorField = vectorField;
        }

        //public Vector3 Curl(Vector3 pos) => Multiplier * UnderlyingVectorField.Curl(pos);

        //public double Divergence(Vector3 pos) => Multiplier * UnderlyingVectorField.Divergence(pos);

        public Vector3 Value(Vector3 pos) => Multiplier * UnderlyingVectorField.Value(pos);
    }
}
