using Math3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A vector field that is the Zero Vector at all points.
    /// </summary>
    public class ZeroVectorField : IVectorField
    {
        public static ZeroVectorField Instance { get { return instance; } }
        private static ZeroVectorField instance = new ZeroVectorField();

        private ZeroVectorField() { } // singleton

        public Vector3 Value(Vector3 pos) => Vector3.Zero;

        public Vector3 Curl(Vector3 pos) => Vector3.Zero;

        public double Divergence(Vector3 pos) => 0;
    }
}
