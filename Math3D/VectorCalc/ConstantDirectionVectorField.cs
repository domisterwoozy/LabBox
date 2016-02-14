using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    public class ConstantDirectionVectorField : SimpleVectorField
    {
        public IScalarField ScalarComponent { get; }
        public Vector3 ConstantVectorComponent { get; }

        public ConstantDirectionVectorField(IScalarField scalarComp, Vector3 vectorComp)
        {
            ScalarComponent = scalarComp;
            ConstantVectorComponent = vectorComp;
        }

        public override Vector3 Value(Vector3 pos) => ScalarComponent.Value(pos) * ConstantVectorComponent;
    }
}
