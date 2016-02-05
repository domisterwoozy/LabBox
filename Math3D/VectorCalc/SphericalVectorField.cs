using Math3D.CoordinateSystems;
using Math3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    public class SphericalVectorField : AbstractVectorField
    {
        public SphericalScalarField R { get; }
        public SphericalScalarField Theta { get; }
        public SphericalScalarField Phi { get; }
        

        public SphericalVectorField(SphericalScalarField r, SphericalScalarField theta, SphericalScalarField phi)
        {
            R = r;            
            Theta = theta;
            Phi = phi;
        }

        public override Vector3 Value(Vector3 pos)
        {
            var vectOrigin = SphericalCoords.System.FromCartesian(pos);
            var spherVect = new Coords3D<SphericalCoords>(SphericalCoords.System, R.Value(pos), Phi.Value(pos), Theta.Value(pos));
            Vector3 test = spherVect.ToCartesianVector(vectOrigin);
            return spherVect.ToCartesianVector(vectOrigin);
        }

        public override Vector3 Curl(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public override double Divergence(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        
    }
}
