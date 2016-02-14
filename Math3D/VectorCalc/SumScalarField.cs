namespace Math3D.VectorCalc
{
    /// <summary>
    /// Represents the sum of two arbitrary scalar fields.
    /// </summary>
    public class SumScalarField : IScalarField
    {
        /// <summary>
        /// The first scalar field to sum.
        /// </summary>
        public IScalarField A { get; }
        /// <summary>
        /// The second scalar field to sum.
        /// </summary>
        public IScalarField B { get; }

        public SumScalarField(IScalarField a, IScalarField b)
        {
            A = a;
            B = b;
        }

        public double Value(Vector3 pos) => A.Value(pos) + B.Value(pos);

        public Vector3 Gradient(Vector3 pos) => A.Gradient(pos) + B.Gradient(pos);

        public IVectorField ToVectorField() => new SumVectorField(A.ToVectorField(), B.ToVectorField());    
    }
}
