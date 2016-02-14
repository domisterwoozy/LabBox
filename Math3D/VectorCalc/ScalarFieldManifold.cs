using Math3D.Geometry;
using System;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A manifold that represents an equipotential of a scalar field.
    /// </summary>
    public class ScalarFieldManifold : AbstractManifold
    {
        private const double DefaultTolerance = 0.01; // need to tune

        public IScalarField UnderlyingScalarField { get; }
        public double PotentialValue { get; }
        public double Tolerance { get; }

        public ScalarFieldManifold(IScalarField s, double potential)
        {
            UnderlyingScalarField = s;
            PotentialValue = potential;
        }

        public override Vector3 MapToManifold(Vector3 point) => UnderlyingScalarField.GradientTraversal(point, PotentialValue, Tolerance);

        public override Vector3 PerpVector(Vector3 pointOnSurface) => UnderlyingScalarField.Gradient(pointOnSurface);

        public override bool IsOnManifold(Vector3 point) => Math.Abs(UnderlyingScalarField.Value(point) - PotentialValue) <= MathConstants.DoubleEqualityTolerance;
    }
}
