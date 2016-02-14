using Math3D;
using Math3D.VectorCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Forces
{
    public static class PhysicsFields
    {
        /// <summary>
        /// The first order approximation as seen here:
        /// https://en.wikipedia.org/wiki/Magnetic_dipole#External_magnetic_field_produced_by_a_magnetic_dipole_moment
        /// </summary>
        public static IVectorField MagneticDipole(Vector3 magMoment, double magConstant)
        {
            Vector3 constantVectOne = (-magConstant / (4 * Math.PI)) * magMoment;
            IVectorField termOne = FieldExtensions.SphericalScalarField(1.0, -3).Multiply(constantVectOne);

            double constantTwo = (magConstant / (4 * Math.PI)) * 3;
            Func<Vector3, Vector3> termTwoFunc = r => Math.Pow(r.Magnitude, -5) * (magMoment * r) * r;
            IVectorField termTwo = new CustomVectorField(termTwoFunc);

            return termOne.Add(termTwo);
        }

        public static IVectorField PointChargeElectric(double strength)
        {
            return FieldExtensions.SphericalScalarField(strength, -1).ToVectorField();
        }

        public static IVectorField PointMassGravity(double strength)
        {
            if (strength < 0) throw new ArgumentException(nameof(strength) + " must be positive");
            return FieldExtensions.SphericalScalarField(-strength, -1).ToVectorField();
        }
    }
}
