using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Geometry;

namespace Math3D.VectorCalc
{
    public class ZeroScalarField : IScalarField
    {
        public static ZeroScalarField Instance { get { return instance; } }
        private static ZeroScalarField instance = new ZeroScalarField();

        public Vector3 Gradient(Vector3 pos) => Vector3.Zero;

        public Vector3 GradientTraversal(Vector3 pos, double desiredPotential, double tolerance)
        {
            throw new InvalidOperationException();
        }

        public IManifold ToManifold(double potential)
        {
            throw new InvalidOperationException();
        }

        public IVectorField ToVectorField() => ZeroVectorField.Instance;

        public double Value(Vector3 pos) => 0;
    }
}
