using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    public class CartesianVectorField : AbstractVectorField
    {
        public CartesianScalarField X { get; }
        public CartesianScalarField Y { get; }
        public CartesianScalarField Z { get; }

        public CartesianVectorField(CartesianScalarField xComp, CartesianScalarField yComp, CartesianScalarField zComp)
        {
            X = xComp;
            Y = yComp;
            Z = zComp;
        }

        public override Vector3 Value(Vector3 point) => new Vector3(X.Value(point), Y.Value(point), Z.Value(point));     

        public override Vector3 Curl(Vector3 point)
        {
            return new Vector3(X.dfdy(point) - Y.dfdz(point),
                                X.dfdz(point) - Z.dfdx(point),
                                Y.dfdx(point) - X.dfdy(point));
        }

        public override double Divergence(Vector3 point) => X.dfdx(point) + Y.dfdy(point) + Z.dfdz(point);        
    }
}
