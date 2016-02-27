using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    public struct PrimitiveTriangle
    {
        public Vertex A { get; }
        public Vertex B { get; }
        public Vertex C { get; }

        public PrimitiveTriangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
