using Math3D;
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
        public Vector3 Normal { get; }

        public PrimitiveTriangle(Vertex a, Vertex b, Vertex c, bool setVertexNormals = true)
        {
            Normal = (b.Pos - a.Pos) ^ (c.Pos - a.Pos);
            if (setVertexNormals)
            {
                A = new Vertex(a.Pos, a.Color, Normal);
                B = new Vertex(b.Pos, b.Color, Normal);
                C = new Vertex(c.Pos, c.Color, Normal);
            }
            else
            {
                A = a;
                B = b;
                C = c;
            }
        }
    }
}
