using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;

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

        public PrimitiveTriangle NewColor(Color c) => new PrimitiveTriangle(A.NewColor(c), B.NewColor(c), C.NewColor(c), false);
    }

    public static class TriangleExtensions
    {
        public static IEnumerable<Vertex> Vertices(this PrimitiveTriangle tri)
        {
            yield return tri.A;
            yield return tri.B;
            yield return tri.C;
        }

        public static IEnumerable<Vertex> Flatten(this IEnumerable<PrimitiveTriangle> tris) => tris.SelectMany(t => t.Vertices());

        public static IEnumerable<PrimitiveTriangle> NewColor(this IEnumerable<PrimitiveTriangle> tris, Color c) => tris.Select(t => t.NewColor(c));
    }
}
