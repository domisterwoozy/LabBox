using Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization
{
    public struct Vertex
    {
        private static readonly Random rand = new Random();

        public Vector3 Pos { get; }
        public Color Color { get; }

        public Vertex(Vector3 pos, Color color)
        {
            Pos = pos;
            Color = color;
        }

        public Vertex(double x, double y, double z)
        {
            Pos = new Vector3(x, y, z);
            Color = Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }
    }

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

    public interface IGraphicalBody
    {
        PrimitiveTriangle[] Triangles { get; }
        Quaternion Orientation { get; set; }
        Vector3 Translation { get; set; }
    }

    public class BasicGraphicalBody : IGraphicalBody
    {
        public Quaternion Orientation { get; set; } = Quaternion.FromRotMatrix(Matrix3.Identity);

        public Vector3 Translation { get; set; } = Vector3.Zero;

        public PrimitiveTriangle[] Triangles { get; }

        public BasicGraphicalBody(IEnumerable<PrimitiveTriangle> tris)
        {
            Triangles = tris.ToArray();
        }
    }

    public static class ShapeFactory
    {
        public static BasicGraphicalBody newCuboid(double x, double y, double z)
        {
            List<Vertex> verts = new List<Vertex>();
            verts.Add(new Vertex(x, y, z));
            verts.Add(new Vertex(-x, y, z));
            verts.Add(new Vertex(-x, -y, z));
            verts.Add(new Vertex(x, -y, z));
            verts.Add(new Vertex(x, y, -z));
            verts.Add(new Vertex(-x, y, -z));
            verts.Add(new Vertex(-x, -y, -z));
            verts.Add(new Vertex(x, -y, -z));

            List<PrimitiveTriangle> prims = new List<PrimitiveTriangle>();
            prims.Add(new PrimitiveTriangle(verts[0], verts[1], verts[2]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[2], verts[3]));
            prims.Add(new PrimitiveTriangle(verts[4], verts[6], verts[5]));
            prims.Add(new PrimitiveTriangle(verts[4], verts[7], verts[6]));
            prims.Add(new PrimitiveTriangle(verts[2], verts[7], verts[3]));
            prims.Add(new PrimitiveTriangle(verts[2], verts[6], verts[7]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[5], verts[1]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[4], verts[5]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[7], verts[4]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[3], verts[7]));
            prims.Add(new PrimitiveTriangle(verts[1], verts[6], verts[2]));
            prims.Add(new PrimitiveTriangle(verts[1], verts[5], verts[6]));

            return new BasicGraphicalBody(prims);
        }
    }
}
