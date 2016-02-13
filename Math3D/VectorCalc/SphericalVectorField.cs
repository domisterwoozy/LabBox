using Math3D.CoordinateSystems;
using System;

namespace Math3D.VectorCalc
{
    public class SphericalVectorField : IVectorField
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

        public Vector3 Value(Vector3 pos)
        {
            var vectOrigin = SphericalCoords.System.FromCartesian(pos);
            var spherVect = new Coords3D<SphericalCoords>(SphericalCoords.System, R.Value(pos), Phi.Value(pos), Theta.Value(pos));
            return spherVect.ToCartesianVector(vectOrigin);
        }
        
        public Vector3 Curl(Vector3 pos)
        {
            // todo: implement curl for spherical coords
            throw new NotImplementedException();
        }

        public double Divergence(Vector3 pos)
        {
            // todo: implement div for spherical coords
            throw new NotImplementedException();
        }
    }
}
