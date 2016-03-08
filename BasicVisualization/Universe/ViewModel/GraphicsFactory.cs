using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D;
using System.Drawing;
using System.Collections.Immutable;

namespace LabBox.Visualization.Universe.ViewModel
{
    public static class FlatFactory
    {
        public static ImmutableArray<PrimitiveTriangle> NewCuboid(double width, double length, double height)
        {
            double x = width / 2;
            double y = length / 2;
            double z = height / 2;

            var verts = ImmutableArray.CreateBuilder<Vertex>();
            // tops four points in ccw order
            verts.Add(new Vertex(x, y, z)); // 0
            verts.Add(new Vertex(-x, y, z)); // 1
            verts.Add(new Vertex(-x, -y, z)); // 2
            verts.Add(new Vertex(x, -y, z)); // 3

            verts.Add(new Vertex(x, y, -z)); // 4
            verts.Add(new Vertex(-x, y, -z)); // 5
            verts.Add(new Vertex(-x, -y, -z)); // 6
            verts.Add(new Vertex(x, -y, -z)); // 7

            var prims = ImmutableArray.CreateBuilder<PrimitiveTriangle>();
            // top
            prims.Add(new PrimitiveTriangle(verts[0], verts[1], verts[2]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[2], verts[3]));
            // bottom
            prims.Add(new PrimitiveTriangle(verts[4], verts[6], verts[5]));
            prims.Add(new PrimitiveTriangle(verts[4], verts[7], verts[6]));
            //front
            prims.Add(new PrimitiveTriangle(verts[2], verts[7], verts[3]));
            prims.Add(new PrimitiveTriangle(verts[2], verts[6], verts[7]));
            // back
            prims.Add(new PrimitiveTriangle(verts[0], verts[5], verts[1]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[4], verts[5]));
            // right
            prims.Add(new PrimitiveTriangle(verts[0], verts[7], verts[4]));
            prims.Add(new PrimitiveTriangle(verts[0], verts[3], verts[7]));
            // left
            prims.Add(new PrimitiveTriangle(verts[1], verts[6], verts[2]));
            prims.Add(new PrimitiveTriangle(verts[1], verts[5], verts[6]));

            return prims.ToImmutable();
        }

        public static ImmutableArray<PrimitiveTriangle> NewWall(double x, double y)
        {
            var color = VisUtil.RandomOpaqueColor();
            var verts = ImmutableArray.CreateBuilder<Vertex>();
            verts.Add(new Vertex(x / 2, y / 2, 0, color, Vector3.K));
            verts.Add(new Vertex(-x / 2, y / 2, 0, color, Vector3.K));
            verts.Add(new Vertex(-x / 2, -y / 2, 0, color, Vector3.K));
            verts.Add(new Vertex(x / 2, -y / 2, 0, color, Vector3.K));

            var prims = ImmutableArray.CreateBuilder<PrimitiveTriangle>();
            prims.Add(new PrimitiveTriangle(verts[0], verts[1], verts[3], false));
            prims.Add(new PrimitiveTriangle(verts[1], verts[2], verts[3], false));

            return prims.ToImmutable();
        }
    }

    public class SphereFactory
    {
        public static ImmutableArray<PrimitiveTriangle> NewSphere(double radius, int level)
        {
            if (level > 5) throw new ArgumentException(nameof(level) + " is to damn high");
            double t = (1.0 + Math.Sqrt(5.0)) / 2.0;
            var verts = ImmutableArray.CreateBuilder<Vertex>();
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

            var faces = ImmutableArray.CreateBuilder<PrimitiveTriangle>();
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
                var faces2 = ImmutableArray.CreateBuilder<PrimitiveTriangle>();
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

            return faces.ToImmutable();
        }

        private static Vertex SetVectorLength(Vertex v, double length) => new Vertex(length * v.Pos.UnitDirection, v.Color, v.Pos.UnitDirection);

        private static Vertex GetMiddlePoint(Vertex a, Vertex b)
        {
            return new Vertex(0.5 * (a.Pos + b.Pos),
                Color.FromArgb((a.Color.A + b.Color.A) / 2, (a.Color.R + b.Color.R) / 2, (a.Color.G + b.Color.G) / 2, (a.Color.B + b.Color.B) / 2), 0.5 * (a.Normal + b.Normal));
        }   
        
    }

}
