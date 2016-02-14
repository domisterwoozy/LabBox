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
        /// The first order approximation as seen here (infinitesimal current loop):
        /// https://en.wikipedia.org/wiki/Magnetic_dipole#External_magnetic_field_produced_by_a_magnetic_dipole_moment
        /// The magConstant is also known as the permeability.
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
        /// <summary>
        /// The first order approximation as seen here (infinitesimal seperation):
        /// https://en.wikipedia.org/wiki/Electric_dipole_moment#Potential_and_field_of_an_electric_dipole
        /// The elecConstant is the permitivity
        /// </summary>
        public static IVectorField ElectricDipole(Vector3 elecMoment, double elecConstant)
        {
            if (elecConstant <= 0) throw new ArgumentException(nameof(elecConstant) + " must be positive");
            Vector3 constantVectOne = (-1 / (4 * Math.PI * elecConstant)) * elecMoment;
            IVectorField termOne = FieldExtensions.SphericalScalarField(1.0, -3).Multiply(constantVectOne);

            double constantTwo = (3 / (4 * Math.PI * elecConstant));
            Func<Vector3, Vector3> termTwoFunc = r => Math.Pow(r.Magnitude, -3) * (elecMoment * r.UnitDirection) * r.UnitDirection;
            IVectorField termTwo = new CustomVectorField(termTwoFunc);

            return termOne.Add(termTwo);
        }

        /// <summary>
        /// Generates a magnetic field based on an arbitrary current.
        /// The current integral can be seen here: https://en.wikipedia.org/wiki/Biot%E2%80%93Savart_law#Electric_currents_.28along_closed_curve.29
        /// </summary>
        public static IVectorField BiotSavartMagnetic(double magConstant, Func<Vector3, Vector3> currentIntegral)
        {
            return new CustomVectorField(currentIntegral).Multiply(magConstant / (4 * Math.PI));
        }

        /// <summary>
        /// The resulting electric field of a point charge. The elecConstant is the permitivity.
        /// </summary>
        public static IVectorField PointChargeElectric(double charge, double elecConstant)
        {
            if (elecConstant <= 0) throw new ArgumentException(nameof(elecConstant) + " must be positive");
            return FieldExtensions.SphericalScalarField(charge / (4 * Math.PI * elecConstant), -1).ToVectorField();
        }

        public static IVectorField PointChargeMagnetic(double charge, double magConstant, Vector3 vel)
        {
            double factor = magConstant * charge / (4 * Math.PI);
            Func<Vector3, Vector3> customFunc = r => (factor / r.MagSquared) * (vel ^ r.UnitDirection);
            return new CustomVectorField(customFunc);
        }

        public static IVectorField PointMassGravity(double strength)
        {
            if (strength < 0) throw new ArgumentException(nameof(strength) + " must be positive");
            return FieldExtensions.SphericalScalarField(-strength, -1).ToVectorField();
        }
    }
}
