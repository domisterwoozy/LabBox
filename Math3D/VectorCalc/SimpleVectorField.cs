using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A vector field with no divergence or curl.
    /// </summary>
    public abstract class SimpleVectorField : IVectorCalcField
    {
        public Vector3 Curl(Vector3 pos) => Vector3.Zero;
        public double Divergence(Vector3 pos) => 0;
        public abstract Vector3 Value(Vector3 pos);
    }
}
