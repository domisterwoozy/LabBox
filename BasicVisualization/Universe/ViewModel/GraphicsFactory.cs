using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D;
using System.Drawing;

namespace LabBox.Visualization.Universe.ViewModel
{
    public static class FlatFactory
    {
        public static PrimitiveTriangle[] NewCuboid(double x, double y, double z, Color c)
        {
            List<Vertex> verts = new List<Vertex>();
            verts.Add(new Vertex(x, y, z, c));
            verts.Add(new Vertex(-x, y, z, c));
            verts.Add(new Vertex(-x, -y, z, c));
            verts.Add(new Vertex(x, -y, z, c));
            verts.Add(new Vertex(x, y, -z, c));
            verts.Add(new Vertex(-x, y, -z, c));
            verts.Add(new Vertex(-x, -y, -z, c));
            verts.Add(new Vertex(x, -y, -z, c));

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

            return prims.ToArray();
        }

        public static PrimitiveTriangle[] NewWall(double x, double y, Color color)
        {
            List<Vertex> verts = new List<Vertex>();
            verts.Add(new Vertex(x / 2, y / 2, 0, color));
            verts.Add(new Vertex(-x / 2, y / 2, 0, color));
            verts.Add(new Vertex(-x / 2, -y / 2, 0, color));
            verts.Add(new Vertex(x / 2, -y / 2, 0, color));

            List<PrimitiveTriangle> prims = new List<PrimitiveTriangle>();
            prims.Add(new PrimitiveTriangle(verts[0], verts[1], verts[3]));
            prims.Add(new PrimitiveTriangle(verts[1], verts[2], verts[3]));

            return prims.ToArray();
        }
    }

    public class SphereFactory
    {
        public static PrimitiveTriangle[] NewSphere(Color color, double radius, int level)
        {
            double t = (1.0 + Math.Sqrt(5.0)) / 2.0;
            List<Vertex> verts = new List<Vertex>();
            verts.Add(SetVectorLength(new Vertex(-1, t, 0), radius));
            verts.Add(SetVectorLength(new Vertex(1, t, 0), radius));
            verts.Add(SetVectorLength(new Vertex(-1, -t, 0), radius));
            verts.Add(SetVectorLength(new Vertex(1, -t, 0), radius));

            verts.Add(SetVectorLength(new Vertex(0, -1, t), radius));
            verts.Add(SetVectorLength(new Vertex(0, 1, t), radius));
            verts.Add(SetVectorLength(new Vertex(0, -1, -t), radius));
            verts.Add(SetVectorLength(new Vertex(0, 1, -t), radius));

            verts.Add(SetVectorLength(new Vertex(t, 0, -1), radius));
            verts.Add(SetVectorLength(new Vertex(t, 0, 1), radius));
            verts.Add(SetVectorLength(new Vertex(-t, 0, -1), radius));
            verts.Add(SetVectorLength(new Vertex(-t, 0, 1), radius));

            List<PrimitiveTriangle> faces = new List<PrimitiveTriangle>();
            // 5 faces around point 0
            faces.Add(new PrimitiveTriangle(verts[0], verts[11], verts[5], false));
            faces.Add(new PrimitiveTriangle(verts[0], verts[5], verts[1], false));
            faces.Add(new PrimitiveTriangle(verts[0], verts[1], verts[7], false));
            faces.Add(new PrimitiveTriangle(verts[0], verts[7], verts[10], false));
            faces.Add(new PrimitiveTriangle(verts[0], verts[10], verts[11], false));

            // 5 adjacent faces
            faces.Add(new PrimitiveTriangle(verts[1], verts[5], verts[9], false));
            faces.Add(new PrimitiveTriangle(verts[5], verts[11], verts[4], false));
            faces.Add(new PrimitiveTriangle(verts[11], verts[10], verts[2], false));
            faces.Add(new PrimitiveTriangle(verts[10], verts[7], verts[6], false));
            faces.Add(new PrimitiveTriangle(verts[7], verts[1], verts[8], false));

            // 5 faces around point 3
            faces.Add(new PrimitiveTriangle(verts[3], verts[9], verts[4], false));
            faces.Add(new PrimitiveTriangle(verts[3], verts[4], verts[2], false));
            faces.Add(new PrimitiveTriangle(verts[3], verts[2], verts[6], false));
            faces.Add(new PrimitiveTriangle(verts[3], verts[6], verts[8], false));
            faces.Add(new PrimitiveTriangle(verts[3], verts[8], verts[9], false));

            // 5 adjacent faces
            faces.Add(new PrimitiveTriangle(verts[4], verts[9], verts[5], false));
            faces.Add(new PrimitiveTriangle(verts[2], verts[4], verts[11], false));
            faces.Add(new PrimitiveTriangle(verts[6], verts[2], verts[10], false));
            faces.Add(new PrimitiveTriangle(verts[8], verts[6], verts[7], false));
            faces.Add(new PrimitiveTriangle(verts[9], verts[8], verts[1], false));

            for (int i = 0; i < level; i++)
            {
                var faces2 = new List<PrimitiveTriangle>();
                foreach (PrimitiveTriangle tri in faces)
                {
                    // replace triangle by 4 triangles
                    Vertex a = SetVectorLength(GetMiddlePoint(tri.A, tri.B), radius);
                    Vertex b = SetVectorLength(GetMiddlePoint(tri.B, tri.C), radius);
                    Vertex c = SetVectorLength(GetMiddlePoint(tri.C, tri.A), radius);

                    faces2.Add(new PrimitiveTriangle(tri.A, a, c, false));
                    faces2.Add(new PrimitiveTriangle(tri.B, b, a, false));
                    faces2.Add(new PrimitiveTriangle(tri.C, c, b, false));
                    faces2.Add(new PrimitiveTriangle(a, b, c, false));
                }
                faces = faces2;
            }

            return faces.ToArray();
        }

        private static Vertex SetVectorLength(Vertex v, double length) => new Vertex(length * v.Pos.UnitDirection, v.Color, v.Pos.UnitDirection);

        private static Vertex GetMiddlePoint(Vertex a, Vertex b)
        {
            return new Vertex(0.5 * (a.Pos + b.Pos),
                Color.FromArgb((a.Color.A + b.Color.A) / 2, (a.Color.R + b.Color.R) / 2, (a.Color.G + b.Color.G) / 2, (a.Color.B + b.Color.B) / 2), 0.5 * (a.Normal + b.Normal));
        }   
        
    }

}
