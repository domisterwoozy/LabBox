using System;

namespace Math3D.VectorCalc
{
    public static class FieldExtensions
    {
        // for some fields when the coefficient is zero the exponent does not matter but still needs to exist and cannot be zero or one
        private const double dummyExponent = 10;

        #region Scalar Field Extensions
        /// <summary>
        /// A scalar field that varies solely with the distance from the origin.
        /// The value of the field is strength*radius^power.
        /// </summary>
        public static IScalarField SphericalField(double strength, double power) => 
            new SphericalScalarField(Tuple.Create(strength, 0.0, 0.0), Tuple.Create(power, dummyExponent, dummyExponent));

        /// <summary>
        /// Returns a new scalar field that is 'clamped' outside a specified volume of influence.
        /// When clamped the value of the field is zero.
        /// </summary>
        public static IScalarField Clamp(this IScalarField f, Func<Vector3, bool> clampFunction) => new ClampedScalarField(f, clampFunction);

        /// <summary>
        /// Adds this scalar field to another.
        /// </summary>
        public static IScalarField Add(this IScalarField f, IScalarField other) => new SumScalarField(f, other);

        /// <summary>
        /// Returns a new scalar field with the origin moved to a specified position.
        /// </summary>
        public static IScalarField Translate(this IScalarField f, Vector3 translation) => new TranslatedScalarField(f, translation);

        /// <summary>
        /// Multiplies this scalar field by a constant value.
        /// </summary>
        public static IScalarField Multiply(this IScalarField f, double mult) => new ProductScalarField(mult, f);
        #endregion

        #region Vector Field Extensions
        /// <summary>
        /// Returns a new vector field that is 'clamped' outside a specified volume of influence.
        /// When clamped the value of the field is zero.
        /// </summary>
        public static IVectorField Clamp(this IVectorField f, Func<Vector3, bool> clampFunction) => new ClampedVectorField(f, clampFunction);

        /// <summary>
        /// Adds this vector field to another.
        /// </summary>
        public static IVectorField Add(this IVectorField f, IVectorField other) => new SumVectorField(f, other);

        /// <summary>
        /// Returns a new vector field with the origin moved to a specified position.
        /// </summary>
        public static IVectorField Translate(this IVectorField f, Vector3 translation) => new TranslatedVectorField(f, translation);

        /// <summary>
        /// Multiplies this vector field by a constant value.
        /// </summary>
        public static IVectorField Multiply(this IVectorField f, double mult) => new ProductVectorField(mult, f);
        #endregion
    }
}
