using Math3D.Geometry;
using System;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A manifold that represents an equipotential of a scalar field.
    /// </summary>
    public class ScalarFieldManifold : AbstractManifold
    {
        public IScalarField UnderlyingScalarField { get; }
        public double PotentialValue { get; }
        public double Tolerance { get; }

        public ScalarFieldManifold(IScalarField s, double potential, double tolerance)
        {
            UnderlyingScalarField = s;
            PotentialValue = potential;
            Tolerance = tolerance;
        }

        public override Vector3 MapToManifold(Vector3 point) => UnderlyingScalarField.GradientTraversal(point, PotentialValue, Tolerance);

        public override Vector3 PerpVector(Vector3 pointOnSurface) => UnderlyingScalarField.Gradient(pointOnSurface);

        public override bool IsOnManifold(Vector3 point) => Math.Abs(UnderlyingScalarField.Value(point) - PotentialValue) <= MathConstants.DoubleEqualityTolerance;
    }
}
