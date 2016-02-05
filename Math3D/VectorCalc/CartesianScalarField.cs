using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Interfaces;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// An implementation of IScalarField of the form f(x,y,z) = A*x^a + B*y^b + C*z^c.
    /// </summary>
    public class CartesianScalarField : AbstractScalarField
    {
        /// <summary>
        /// X componenet coefficient.
        /// </summary>
        public double A { get; }
        /// <summary>
        /// Y componenet coefficient.
        /// </summary>
        public double B { get; }
        /// <summary>
        /// Z componenet coefficient.
        /// </summary>
        public double C { get; }

        /// <summary>
        /// X component exponent.
        /// </summary>
        public double a { get; }
        /// <summary>
        /// Y component exponent.
        /// </summary>
        public double b { get; }
        /// <summary>
        /// Z component exponent.
        /// </summary>
        public double c { get; }       
         
        /// <summary>  
	    /// Creates a scaler field of the form
	    /// f(x,y,z) = A*x^a + B*y^b + C*z^c.
        /// </summary>
        /// <param name="coefficients">Coefficients of the scalar field (A, B, C). Must be of length 3.</param>
        /// <param name="exponents">Exponents of the scalar field (a, b, c). Must be of length 3.</param>
        public CartesianScalarField(Tuple<double, double, double> coefficients, Tuple<double, double, double> exponents)
        {
            if (exponents.Item1 == 1.0 || exponents.Item2 == 1.0 || exponents.Item3 == 1.0)
            {
                throw new ArgumentException("You cannot currently enter an exponent equal to one. Support for exponents of one will be coming soon.");
            }
            if (exponents.Item1 == 0.0 || exponents.Item2 == 0.0 || exponents.Item3 == 0.0)
            {
                throw new ArgumentException("You cannot currently enter an exponent equal to zero. Support for exponents of one will be coming soon.");
            }

            A = coefficients.Item1;
            B = coefficients.Item2;
            C = coefficients.Item3;
            a = exponents.Item1;
            b = exponents.Item2;
            c = exponents.Item3;
        }

        public override double Value(Vector3 point) => A * Math.Pow(point.X, a) + B * Math.Pow(point.Y, b) + C * Math.Pow(point.Z, c);       

        public override Vector3 Gradient(Vector3 point) => new Vector3(dfdx(point), dfdy(point), dfdz(point));        

        public override IVectorField ToVectorField()
        {
            return new CartesianVectorField(new CartesianScalarField(Tuple.Create(-a * A, 0.0, 0.0), Tuple.Create(a - 1, 0.0, 0.0)),
                    new CartesianScalarField(Tuple.Create(0.0, -b * B, 0.0), Tuple.Create(0.0, b - 1, 0.0)),
                    new CartesianScalarField(Tuple.Create(0.0, 0.0, -c * C), Tuple.Create(0.0, 0.0, c - 1)));
        }

        /// <summary>
        /// Derivative of the scalar field with respect to x.
        /// </summary>
        public double dfdx(Vector3 point) => a * A * Math.Pow(point.X, a - 1);
        

        /// <summary>
        /// Derivative of the scalar field with respect to y.
        /// </summary>
        public double dfdy(Vector3 point) => b * B * Math.Pow(point.Y, b - 1);
        

        /// <summary>
        /// Derivative of the scalar field with respect to z.
        /// </summary>
        public double dfdz(Vector3 point) => c * C * Math.Pow(point.Z, c - 1);
        
    }
}
