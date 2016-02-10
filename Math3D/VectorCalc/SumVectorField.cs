namespace Math3D.VectorCalc
{
    /// <summary>
    /// Represents the sum of two vector fields.
    /// </summary>
    public class SumVectorField : AbstractVectorField
    {
        /// <summary>
        /// The first vector field to sum.
        /// </summary>
        public IVectorField A { get; }
        /// <summary>
        /// The second vector field to sum.
        /// </summary>
        public IVectorField B { get; }

        public SumVectorField(IVectorField a, IVectorField b)
        {
            A = a;
            B = b;
        }

        public override Vector3 Value(Vector3 pos) => A.Value(pos) + B.Value(pos);

        public override Vector3 Curl(Vector3 pos) => A.Curl(pos) + B.Curl(pos);

        public override double Divergence(Vector3 pos) => A.Divergence(pos) + B.Divergence(pos); 
    }
}
