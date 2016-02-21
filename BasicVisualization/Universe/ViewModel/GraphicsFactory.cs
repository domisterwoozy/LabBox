using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D;

namespace BasicVisualization.Universe.ViewModel
{
    public static class GraphicsFactory
    {
        public static PrimitiveTriangle[] NewCuboid(double x, double y, double z)
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

            return prims.ToArray();
        }
    }

    /// <summary>
    /// A simple body that is backed by a body from the physics engine.
    /// </summary>
    public class BasicGraphicalBody : IGraphicalBody
    {
        public IBody Body { get; }

        public Quaternion Orientation => Body.Dynamics.Transform.Q;
        public Vector3 Translation => Body.Dynamics.Transform.Pos;

        public PrimitiveTriangle[] Triangles { get; }

        public BasicGraphicalBody(IBody body)
        {
            Body = body;
            Triangles = GraphicsFactory.NewCuboid(1, 1, 1);
        }
    }
}
