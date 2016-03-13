using Math3D;
using Physics3D.Bodies;
using Physics3D.Universes;
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

    /// <summary>
    /// Smoothes out the kinematics of a body by performing a sample over a specified number of frames.
    /// </summary>
    public class SmoothedGraphicalBody : IGraphicalBody
    {
        private readonly Queue<Transform> pastTransforms;
        private Vector3 posSum;
        private Quaternion orientationSum;

        public IBody Body { get; }

        public int FramesToSmooth { get; set; } = 10;

        //public Vector3 Translation { get; private set; }
        //public Quaternion Orientation { get; private set; }

        public Vector3 Translation => (1.0 / pastTransforms.Count) * posSum;
        public Quaternion Orientation => ((1.0 / pastTransforms.Count) * orientationSum).Normalized();

        public ImmutableArray<PrimitiveTriangle> Triangles { get; }

        public SmoothedGraphicalBody(IBody body, ImmutableArray<PrimitiveTriangle> tris, int framesToSmooth)
        {
            Body = body;
            Triangles = tris;
            FramesToSmooth = framesToSmooth;
            pastTransforms = new Queue<Transform>(FramesToSmooth);
            AddTransform(Body.Dynamics.Kinematics.Transform); // add starting transform to queue

            Body.FrameFinished += Body_FrameFinished;
        }

        private void Body_FrameFinished(object sender, FrameLengthArgs e)
        {
            if (pastTransforms.Count > FramesToSmooth) RemoveTransform();
            AddTransform(Body.Dynamics.Kinematics.Transform);                      

            //Translation = pastTransforms.Average(t => t.Pos);
            //Orientation = pastTransforms.Average(t => t.Q);
        }

        private void RemoveTransform()
        {
            Transform removed = pastTransforms.Dequeue();
            posSum -= removed.Pos;
            orientationSum -= removed.Q;
        }
        private void AddTransform(Transform t)
        {
            pastTransforms.Enqueue(t);
            posSum += t.Pos;
            orientationSum += t.Q;
        }

        public IGraphicalBody NewColor(Color c) => new SmoothedGraphicalBody(Body, Triangles.NewColor(c).ToImmutableArray(), FramesToSmooth);
        public IGraphicalBody NewShape(ImmutableArray<PrimitiveTriangle> tris) => new SmoothedGraphicalBody(Body, tris, FramesToSmooth);
    }
}
