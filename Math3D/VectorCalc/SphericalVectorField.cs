using Math3D.CoordinateSystems;
using System;

namespace Math3D.VectorCalc
{
    public class SphericalVectorField : IVectorField
    {
        public IScalarField R { get; }
        public IScalarField Theta { get; }
        public IScalarField Phi { get; }        

        public SphericalVectorField(IScalarField r, IScalarField theta, IScalarField phi)
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
        
        //public Vector3 Curl(Vector3 pos)
        //{
        //    // todo: implement curl for spherical coords
        //    throw new NotImplementedException();
        //}

        //public double Divergence(Vector3 pos)
        //{
        //    // todo: implement div for spherical coords
        //    throw new NotImplementedException();
        //}
    }
}
