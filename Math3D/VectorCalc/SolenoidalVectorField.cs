using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A divergence free vector field. Sometimes called 'incompressible'.
    /// </summary>
    public abstract class SolenoidalVectorField : IVectorCalcField
    {
        public abstract Vector3 Curl(Vector3 pos);
        public double Divergence(Vector3 pos) => 0;
        public abstract Vector3 Value(Vector3 pos);
    }
}
