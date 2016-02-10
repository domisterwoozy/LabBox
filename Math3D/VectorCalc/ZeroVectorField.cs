namespace Math3D.VectorCalc
{
    /// <summary>
    /// A vector field that is the Zero Vector at all points.
    /// </summary>
    public class ZeroVectorField : ConservativeVectorField
    {
        public static ZeroVectorField Instance { get { return instance; } }
        private static ZeroVectorField instance = new ZeroVectorField();

        private ZeroVectorField() { } // singleton

        public override Vector3 Value(Vector3 pos) => Vector3.Zero;

        public override double Divergence(Vector3 pos) => 0;
    }
}
