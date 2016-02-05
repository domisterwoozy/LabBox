using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Interfaces;
using Math3D.CoordinateSystems;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A generic scalar field in the spherical coordinate system (r, theta, phi)
    /// f(r,theta,phi) = A*r^a + B*theta^b + C*phi^c.
    /// </summary>
    public class SphericalScalarField : AbstractScalarField
    {
        private const double arbitraryZeroExponent = 10; // if the coeffient is zero the exponent can be anything besides zero or one

        /// <summary>
        /// r componenet coefficient.
        /// </summary>
        public double A { get; }
        /// <summary>
        /// phi componenet coefficient.
        /// </summary>
        public double B { get; }
        /// <summary>
        /// theta componenet coefficient.
        /// </summary>
        public double C { get; }

        /// <summary>
        /// r component exponent.
        /// </summary>
        public double a { get; }
        /// <summary>
        /// phi component exponent.
        /// </summary>
        public double b { get; }
        /// <summary>
        /// theta component exponent.
        /// </summary>
        public double c { get; }

        /// <summary>  
        /// Creates a scaler field of the form
        /// f(r,theta,phi) = A*r^a + B*theta^b + C*phi^c.
        /// </summary>
        /// <param name="coefficients">Coefficients of the scalar field (A, B, C)</param>
        /// <param name="exponents">Exponents of the scalar field. Can only be equal to zero or one if the corresponding coefficient is zero. (a, b, c)</param>
        public SphericalScalarField(Tuple<double, double, double> coefficients, Tuple<double, double, double> exponents)
        {
            double[] modifiedExponents = new[] { exponents.Item1, exponents.Item2, exponents.Item3 };
            if (coefficients.Item1 == 0.0) modifiedExponents[0] = arbitraryZeroExponent;
            if (coefficients.Item2 == 0.0) modifiedExponents[1] = arbitraryZeroExponent;
            if (coefficients.Item3 == 0.0) modifiedExponents[2] = arbitraryZeroExponent;

            if (modifiedExponents[0] == 1.0 || modifiedExponents[1] == 1.0 || modifiedExponents[2] == 1.0)
            {
                throw new ArgumentException("You cannot currently enter an exponent equal to one. Support for exponents of one will be coming soon.");
            }
            if (modifiedExponents[0] == 0.0 || modifiedExponents[1] == 0.0 || modifiedExponents[2] == 0.0)
            {
                throw new ArgumentException("You cannot currently enter an exponent equal to zero. Support for exoponents of one will be coming soon.");
            }

            A = coefficients.Item1;
            B = coefficients.Item2;
            C = coefficients.Item3;
            a = exponents.Item1;
            b = exponents.Item2;
            c = exponents.Item3;
        }

        public override double Value(Vector3 point)
        {
            var spherCoords = SphericalCoords.System.FromCartesian(point);
            return A * Math.Pow(spherCoords.FirstComponent, a) + B * Math.Pow(spherCoords.SecondComponent, b) + C * Math.Pow(spherCoords.ThirdComponent, c);
        }

        public override Vector3 Gradient(Vector3 point)
        {
            Coords3D<SphericalCoords> coords = SphericalCoords.System.FromCartesian(point);
            var gradient = new Coords3D<SphericalCoords>(SphericalCoords.System, 
                dfdr(coords),
                dfdtheta(coords) / (coords.FirstComponent * Math.Sin(coords.ThirdComponent)),
                dfdphi(coords) / coords.FirstComponent
            );
            return SphericalCoords.System.ToCartesian(gradient);
        }

        public override IVectorField ToVectorField()
        {
            return new SphericalVectorField(
                new SphericalScalarField(Tuple.Create(-a * A, 0.0, 0.0), Tuple.Create(a - 1, 0.0, 0.0)),
                new SphericalScalarField(Tuple.Create(0.0, -b * B, 0.0), Tuple.Create(0.0, b - 1, 0.0)),
                new SphericalScalarField(Tuple.Create(0.0, 0.0, -c * C), Tuple.Create(0.0, 0.0, c - 1))
            );
        }

        /// <summary>
        /// Derivative of the scalar field with respect to r.
        /// </summary>
        public double dfdr(Coords3D<SphericalCoords> point) => a * A * Math.Pow(point.FirstComponent, a - 1);

        /// <summary>
        /// Derivative of the scalar field with respect to theta.
        /// </summary>
        public double dfdtheta(Coords3D<SphericalCoords> point) => b * B * Math.Pow(point.SecondComponent, b - 1);

        /// <summary>
        /// Derivative of the scalar field with respect to phi.
        /// </summary>
        public double dfdphi(Coords3D<SphericalCoords> point) => c * C * Math.Pow(point.ThirdComponent, c - 1);
        
        ///// <summary>
        ///// Derivative of r times the scalar field with respect to r.
        ///// </summary>
        //public double drfdr(Coords3D<SphericalCoords> point) => (a + 1) * A * Math.Pow(point.FirstComponent, a);

        ///// <summary>
        ///// Derivative of r squared times the scalar field with respect to r. d(r^2f)/dr
        ///// </summary>
        //public double dr2fdr(Coords3D<SphericalCoords> point) => (a + 2) * A * Math.Pow(point.FirstComponent, a + 1);

        ///// <summary>
        ///// Derivative of sin(theta) times the field with respect to theta.
        ///// </summary>
        //public double dsinthetafdtheta(Coords3D<SphericalCoords> point)
        //{
        //    double cosTheta = Math.Cos(point.ThirdComponent);
        //    return A * cosTheta * Math.Pow(point.FirstComponent, a) + B * cosTheta * Math.Pow(point.SecondComponent, b) +
        //        C * cosTheta * Math.Pow(point.ThirdComponent, c) + c * C * Math.Sin(point.ThirdComponent) * Math.Pow(point.ThirdComponent, c - 1);
        //}
    }
}
