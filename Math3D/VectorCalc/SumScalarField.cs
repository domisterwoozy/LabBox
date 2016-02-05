using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Interfaces;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// Represents the sum of two arbitrary scalar fields.
    /// </summary>
    public class SumScalarField : AbstractScalarField
    {
        /// <summary>
        /// The first scalar field to sum.
        /// </summary>
        public IScalarField A { get; }
        /// <summary>
        /// The second scalar field to sum.
        /// </summary>
        public IScalarField B { get; }

        public SumScalarField(IScalarField a, IScalarField b)
        {
            A = a;
            B = b;
        }

        public override double Value(Vector3 pos) => A.Value(pos) + B.Value(pos);

        public override Vector3 Gradient(Vector3 pos) => A.Gradient(pos) + B.Gradient(pos);

        public override IVectorField ToVectorField() => new SumVectorField(A.ToVectorField(), B.ToVectorField());    
    }
}
