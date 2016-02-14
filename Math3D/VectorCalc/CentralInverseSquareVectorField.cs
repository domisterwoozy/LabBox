namespace Math3D.VectorCalc
{
    /// <summary>
    /// A radially symetric center seeking vector field of the form
    /// Constant / r^2
    /// Where r is the distance between the field source position and a
    /// position in the field.
    /// </summary>
    public class CentralInverseSquareVectorField : IVectorCalcField
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
            if (pos == SourcePosition)
            {
                if (ConstantFactor == 0) return 0;
                else if (ConstantFactor > 0) return double.PositiveInfinity;
                else return double.NegativeInfinity;
            }
            else return 0;
        }
    }
}
