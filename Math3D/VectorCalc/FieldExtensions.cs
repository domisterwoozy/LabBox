using System;

namespace Math3D.VectorCalc
{
    public static class FieldExtensions
    {
        // when the coefficient is zero the exponent does not matter but still needs to exist and cannot be zero or one
        private const double dummyExponent = 10; 

        /// <summary>
        /// A scalar field that varies solely with the distance from the origin.
        /// The value of the field is strength*radius^power.
        /// </summary>
        public static IScalarField SphericalField(double strength, double power) => 
            new SphericalScalarField(Tuple.Create(strength, 0.0, 0.0), Tuple.Create(power, dummyExponent, dummyExponent));

        /// <summary>
        /// Returns a new scalar field only active inside a specified radius of influence.
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
    }
}
