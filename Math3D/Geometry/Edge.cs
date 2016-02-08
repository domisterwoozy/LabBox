using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public struct Edge
    {
        public Vector3 A { get; }
        public Vector3 B { get; }

        public Edge(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
        }
    }
}
