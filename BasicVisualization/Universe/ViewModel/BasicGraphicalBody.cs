using Math3D;
using Physics3D.Bodies;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    /// <summary>
    /// A simple body that is backed by a body from the physics engine.
    /// </summary>
    public class BasicGraphicalBody : IGraphicalBody
    {
        public IBody Body { get; }

        public Quaternion Orientation => Body.Dynamics.Transform.Q;
        public Vector3 Translation => Body.Dynamics.Transform.Pos;

        public ImmutableArray<PrimitiveTriangle> Triangles { get; }

        public BasicGraphicalBody(IBody body) : this(body, SphereFactory.NewSphere(1, 4)) { }

        public BasicGraphicalBody(IBody body, ImmutableArray<PrimitiveTriangle> tris)
        {
            Body = body;
            Triangles = tris;
        }

        public static IEnumerable<BasicGraphicalBody> FromPhysicsBodies(IEnumerable<IBody> bodies)
        {
            foreach (var b in bodies) yield return new BasicGraphicalBody(b);
        }

        public IGraphicalBody NewColor(Color c) => new BasicGraphicalBody(Body, Triangles.NewColor(c).ToImmutableArray());
        public IGraphicalBody NewShape(ImmutableArray<PrimitiveTriangle> tris) => new BasicGraphicalBody(Body, tris);    
    }
}
