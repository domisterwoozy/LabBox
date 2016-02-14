using Math3D.Geometry;
using System;
using Util;

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
        public static SphericalScalarField SphericalScalarField(double strength, double power) => 
            new SphericalScalarField(Tuple.Create(strength, 0.0, 0.0), Tuple.Create(power, dummyExponent, dummyExponent));

        /// <summary>
        /// Returns a new scalar field that is 'clamped' outside a specified volume of influence.
        /// When clamped the value of the field is zero.
        /// </summary>
        public static ClampedScalarField Clamp(this IScalarField f, Func<Vector3, bool> clampFunction) => new ClampedScalarField(f, clampFunction);

        /// <summary>
        /// Adds this scalar field to another.
        /// </summary>
        public static SumScalarField Add(this IScalarField f, IScalarField other) => new SumScalarField(f, other);

        /// <summary>
        /// Returns a new scalar field with the origin moved to a specified position.
        /// </summary>
        public static TranslatedScalarField Translate(this IScalarField f, Vector3 translation) => new TranslatedScalarField(f, () => translation);

        /// <summary>
        /// Returns a new scalar field with the origin moved by a specified translation generator.
        /// </summary>
        public static TranslatedScalarField Translate(this IScalarField f, Generator<Vector3> translation) => new TranslatedScalarField(f, translation);

        /// <summary>
        /// Multiplies this scalar field by a constant value.
        /// </summary>
        public static ProductScalarField Multiply(this IScalarField f, double mult) => new ProductScalarField(f, mult);
        #endregion

        #region Vector Field Extensions
        /// <summary>
        /// A scalar field that varies solely with the distance from the origin.
        /// The value of the field is strength*radius^power.
        /// </summary>
        public static IVectorField SphericalVectorField(double strength, double power) =>
            new SphericalScalarField(Tuple.Create(strength, 0.0, 0.0), Tuple.Create(power + 1, dummyExponent, dummyExponent)).ToVectorField();

        /// <summary>
        /// Returns a new vector field that is 'clamped' outside a specified volume of influence.
        /// When clamped the value of the field is zero.
        /// </summary>
        public static ClampedVectorField Clamp(this IVectorField f, Func<Vector3, bool> clampFunction) => new ClampedVectorField(f, clampFunction);

        /// <summary>
        /// Adds this vector field to another.
        /// </summary>
        public static SumVectorField Add(this IVectorField f, IVectorField other) => new SumVectorField(f, other);

        /// <summary>
        /// Returns a new vector field with the origin moved to a specified position.
        /// </summary>
        public static TranslatedVectorField Translate(this IVectorField f, Vector3 translation) => new TranslatedVectorField(f, () => translation);

        /// <summary>
        /// Returns a new vector field with the origin moved by a specified generation function.
        /// </summary>
        public static TranslatedVectorField Translate(this IVectorField f, Generator<Vector3> translation) => new TranslatedVectorField(f, translation);

        /// <summary>
        /// Multiplies this vector field by a constant value.
        /// </summary>
        public static ProductVectorField Multiply(this IVectorField f, double mult) => new ProductVectorField(f, mult);

        public static ConstantDirectionVectorField Multiply(this IScalarField sf, Vector3 vectorComp) => new ConstantDirectionVectorField(sf, vectorComp);
        #endregion

        /// <summary>
        /// Traverses a path of steepest ascent/descent until the desired potential is reached.
        /// </summary>
        /// <param name="pos">Starting point</param>
        /// <param name="desiredPotential">The field value you want to reach</param>
        /// <returns>The position the traversal ends at</returns>
        public static Vector3 GradientTraversal(this IScalarField field, Vector3 pos, double desiredPotential, double tolerance)
        {
            const double stepSize = 0.5; // TODO: tune this

            Vector3 currentPos = pos;
            double potentialDiff = desiredPotential - field.Value(currentPos);

            while (Math.Abs(potentialDiff) >= tolerance)
            {
                currentPos = currentPos + (stepSize * potentialDiff) * field.Gradient(pos);
                potentialDiff = desiredPotential - field.Value(currentPos);
            }

            return pos;
        }

        /// <summary>
        /// Generates a manifold from this scalar field at a certain potential.
        /// This is an equipotential surface of this scalar field.
        /// </summary>
        public static ScalarFieldManifold ToManifold(this IScalarField field, double potential) => new ScalarFieldManifold(field, potential);
    }
}
