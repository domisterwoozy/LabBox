namespace Math3D.VectorCalc
{
    /// <summary>
    /// A wrapper that allows you to move a vector field in 3D space.
    /// </summary>
    public class TranslatedVectorField : AbstractVectorField
    {
        /// <summary>
        /// The underlying vector field to move around.
        /// </summary>
        public IVectorField UnderlyingVectorField { get; }

        /// <summary>
        /// The position in 3D space of the underlying vector field.
        /// </summary>
        public Vector3 Position { get; }

        public TranslatedVectorField(IVectorField underlyingVectField, Vector3 translation)
        {
            UnderlyingVectorField = underlyingVectField;
            Position = translation;
        }

        public override Vector3 Curl(Vector3 pos) => UnderlyingVectorField.Curl(pos - Position);

        public override double Divergence(Vector3 pos) => UnderlyingVectorField.Divergence(pos - Position);

        public override Vector3 Value(Vector3 pos) => UnderlyingVectorField.Value(pos - Position);
    }
}
