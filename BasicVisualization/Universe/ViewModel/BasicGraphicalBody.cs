using Math3D;
using Physics3D.Bodies;
using System;
using System.Collections.Generic;
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

        public PrimitiveTriangle[] Triangles { get; }

        public BasicGraphicalBody(IBody body, Color c)
        {
            Body = body;
            Triangles = SphereFactory.NewSphere(c, 2, 3);
            //Triangles = FlatFactory.NewCuboid(1, 1, 1);
        }

        public static IEnumerable<BasicGraphicalBody> FromPhysicsBodies(IEnumerable<IBody> bodies, Color c)
        {
            foreach (var b in bodies) yield return new BasicGraphicalBody(b, c);
        }
    }
}
