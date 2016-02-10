using Math3D.Geometry;
using System;

namespace Math3D.VectorCalc
{
    public abstract class AbstractScalarField : IScalarField
    {
        private const double DefaultTolerance = 0.01; // need to tune

        public abstract Vector3 Gradient(Vector3 pos);            
        public abstract IVectorField ToVectorField();
        public abstract double Value(Vector3 pos);

        public virtual IManifold ToManifold(double potential) => new ScalarFieldManifold(this, potential, DefaultTolerance);

        public virtual Vector3 GradientTraversal(Vector3 pos, double desiredPotential, double tolerance)
        {
            const double stepSize = 0.5; // TODO: tune this

            Vector3 currentPos = pos;
            double potentialDiff = desiredPotential - Value(currentPos);            

            while (Math.Abs(potentialDiff) >= tolerance)
            {
                currentPos = currentPos + (stepSize * potentialDiff) * Gradient(pos);
                potentialDiff = desiredPotential - Value(currentPos);
            }

            return pos;
        }
    }
}
