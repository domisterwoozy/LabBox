using Math3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    public abstract class AbstractVectorField : IVectorField
    {
        public abstract Vector3 Curl(Vector3 pos);
        public abstract double Divergence(Vector3 pos);
        public abstract Vector3 Value(Vector3 pos);
    }
}
