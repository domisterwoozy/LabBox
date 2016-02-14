using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    public class CustomVectorField : IVectorField
    {
        public Func<Vector3, Vector3> CustomFunc { get; }

        public CustomVectorField(Func<Vector3, Vector3> customFunc)
        {
            CustomFunc = customFunc;
        }

        public Vector3 Value(Vector3 pos) => CustomFunc(pos);
    }
}
