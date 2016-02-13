using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    public class ConstantVectorField : SimpleVectorField
    {
        public Vector3 ConstantVector { get; }

        public ConstantVectorField(Vector3 v)
        {
            ConstantVector = v;
        }

        public override Vector3 Value(Vector3 pos) => ConstantVector;
    }
}
