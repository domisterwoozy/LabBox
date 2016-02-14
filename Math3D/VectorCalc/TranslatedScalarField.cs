using Util;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A wrapper that allows you to move a scalar field in the world.
    /// </summary>
    public class TranslatedScalarField : IScalarField
    {
        private readonly Generator<Vector3> translationGen;
        /// <summary>
        /// The underlying scalar field that can be translated.
        /// </summary>
        public IScalarField UnderlyingScalarField { get; }

        /// <summary>
        /// The position in 3D space of the underlying scalar field.
        /// </summary>
        public Vector3 Position => translationGen();

        public TranslatedScalarField(IScalarField underlyingScalarField, Generator<Vector3> translationGen)
        {
            UnderlyingScalarField = underlyingScalarField;
            this.translationGen = translationGen;
        }

        public double Value(Vector3 pos) => UnderlyingScalarField.Value(pos - Position);

        public Vector3 Gradient(Vector3 pos) => UnderlyingScalarField.Gradient(pos - Position);

        public IVectorField ToVectorField() => new TranslatedVectorField(UnderlyingScalarField.ToVectorField(), translationGen);
    }
}
