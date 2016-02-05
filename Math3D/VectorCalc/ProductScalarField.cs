using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Interfaces;

namespace Math3D.VectorCalc
{
    public class ProductScalarField : AbstractScalarField
    {
        public override Vector3 Gradient(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public override IVectorField ToVectorField()
        {
            throw new NotImplementedException();
        }

        public override double Value(Vector3 pos)
        {
            throw new NotImplementedException();
        }
    }
}
