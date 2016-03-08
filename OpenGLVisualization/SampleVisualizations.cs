using LabBox.Visualization.Universe;
using LabBox.Visualization.Universe.ViewModel;
using Math3D;
using Math3D.Geometry;
using Math3D.VectorCalc;
using Physics3D.Bodies;
using Physics3D.Collisions;
using Physics3D.Forces;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace LabBox.OpenGLVisualization
{
    public static class SampleVisualizations
    {        
        public static OpenGLVisualization Vis()
        {
            var sun = LightSource.Directional(new Vector3(1, 1, -1));
            sun.CastsDynamicShadows = true;
            var light2 = LightSource.SpotLight(new Vector3(10, 0, 5), new Vector3(0, 0, -1), Math.PI / 4);
            light2.CastsDynamicShadows = true;

            IEnumerable<IBody> box = BodyFactory.Box(30, 30, 10, Vector3.Zero);
            var floorGraphic = new MoveableBody(FlatFactory.NewCuboid(30, 30, 1.0).NewColor(Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };

            var balls = new List<IBody>();
            var ball = BodyFactory.SphereMass(1, 1, new Vector3(2, 2, 2), new Vector3(1.1, -2, 4));
            ball.Material.Epsilon = 0.5f;
            balls.Add(ball);      

            var uni = new BasicUniverse();
            uni.ContactResolver = new LoopingContactResolver(new ImpulseCollisionEngine());
            uni.ContactFinder = new BasicContactFinder();
            uni.Bodies.Add(box.ToArray());
            uni.Bodies.Add(balls.ToArray());
            uni.ForceFields.Add(new ForceField(new ConstantVectorField(-9.8 * Vector3.K), ForceFieldFactory.GravityForceApplier));

            var vis = new OpenGLVisualization(balls.Select(b => new BasicGraphicalBody(b, SphereFactory.NewSphere(1.0, 3))).Union<IGraphicalBody>(floorGraphic), sun, light2);
            var physicsRunner = new PausablePhysicsRunner(uni);
            vis.InputHandler.Pause.Start += (sender, e) => physicsRunner.TogglePause();
            vis.InputHandler.Exit.Start += (sender, e) => vis.EndVis();
            vis.VisStarted += (sender, e) => physicsRunner.StartPhysics();

            return vis;
        }

        public static OpenGLVisualization Test()
        {
            MoveableBody floor = new MoveableBody(FlatFactory.NewCuboid(30, 30, 0.5).NewColor(Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };
            throw new NotImplementedException();
        }
    }
}
