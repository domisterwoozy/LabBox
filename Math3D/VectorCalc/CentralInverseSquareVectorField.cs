using Math3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A radially symetric center seeking vector field of the form
    /// Constant / r^2
    /// Where r is the distance between the field source position and a
    /// position in the field.
    /// </summary>
    public class CentralInverseSquareVectorField : IVectorField
    {
        public double ConstantFactor { get; set; }

        public Vector3 SourcePosition { get; set; }

        public CentralInverseSquareVectorField(double constantFactor, Vector3 sourcePosition)
        {
            ConstantFactor = constantFactor;
            SourcePosition = sourcePosition;
        }

        public Vector3 Value(Vector3 pos)
        {
            Vector3 rVect = pos - SourcePosition;
            double mag = ConstantFactor / rVect.MagSquared;
            return mag * rVect.UnitDirection;
        }

        public Vector3 Curl(Vector3 pos) => Vector3.Zero;

        public double Divergence(Vector3 pos)
        {
            if (pos == SourcePosition) return double.PositiveInfinity;
            else return 0;
        }
    }
}
