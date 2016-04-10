using LabBox.Visualization.Input;
using LabBox.Visualization.Universe;
using LabBox.Visualization.Universe.Interaction;
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
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace LabBox.OpenGLVisualization
{
    public static class SampleVisualizations
    {        
        public static OpenGLVisualization BouncyGravity()
        {
            var sun = LightSource.Directional(new Vector3(1, 1, -1));
            sun.CastsDynamicShadows = true;
            var light2 = LightSource.SpotLight(new Vector3(10, 0, 5), new Vector3(0, 0, -1), Math.PI / 4);
            light2.CastsDynamicShadows = true;

            //IEnumerable<IBody> box = BodyFactory.Box(30, 30, 10, Vector3.Zero);
            var floorGraphic = new MoveableBody(FlatFactory.NewCuboid(30, 30, 1.0).NewColor(Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };

            //var balls = new List<IBody>();
            //var ball = BodyFactory.SphereMass(1, 1, new Vector3(2, 2, 2), new Vector3(1.1, -2, 4));
            //ball.Material.Epsilon = 0.5f;
            //balls.Add(ball);      

            var uni = SampleUniverses.BouncyGravity(10, 1000);
            //uni.ForceFields.Add(new ForceField(new ConstantVectorField(-9.8 * Vector3.K), ForceFieldFactory.GravityForceApplier));
            var bodyGraphics = uni.Bodies.Take(5).Select(b => new BasicGraphicalBody(b, SphereFactory.NewSphere(1.0, 3)));
            var bigBodyGraphic = new BasicGraphicalBody(uni.Bodies.Last(), SphereFactory.NewSphere(3, 3));

            var vis = new OpenGLVisualization(bodyGraphics.Union(bigBodyGraphic).Union<IGraphicalBody>(floorGraphic), sun, light2);

            uni.ContactResolver = new LoopingContactResolver(new ImpulseCollisionEngine() { CollidingThresholdSpeed = 0.1f });
            uni.ContactFinder = new BasicContactFinder();

            var physicsRunner = new RealTimePhysicsRunner(uni);
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Pause && inpt.State == InputState.Start).Subscribe(inpt => physicsRunner.TogglePause());
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Exit).Subscribe(inpt => vis.EndVis());
            vis.VisStarted += (sender, e) => physicsRunner.StartPhysics();

            return vis;
        }

        public static OpenGLVisualization BouncyBalls()
        {
            var sun = LightSource.Directional(new Vector3(1, 1, -1));
            sun.CastsDynamicShadows = true;
            var light2 = LightSource.SpotLight(new Vector3(10, 0, 5), new Vector3(0, 0, -1), Math.PI / 4);
            light2.CastsDynamicShadows = true;

            var floor = BodyFactory.Cuboid(30, 30, 1, new Vector3(0, 0, -5));
            floor.Dynamics.Fix();
            var floorGraphic = new MoveableBody(FlatFactory.NewCuboid(30, 30, 1.0).NewColor(Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };

            //var balls = new List<IBody>();
            //var ball = BodyFactory.SphereMass(1, 1, new Vector3(2, 2, 2), new Vector3(1.1, -2, 4));
            //ball.Material.Epsilon = 0.5f;
            //balls.Add(ball);

            //var uni = SampleUniverses.BouncyBalls(10, 10);
            //var bodyGraphics = uni.Bodies.Select(b => new BasicGraphicalBody(b, SphereFactory.NewSphere(1.0, 3))).ToArray();

            var boxOne = BodyFactory.Cuboid(3, 3, 0.5, new Vector3(3, 3, 2), Vector3.Zero, new Vector3(0.1, 0.1, 0.1));
            boxOne.Material.Epsilon = 0.25f;
            boxOne.Material.DynamicFrictionCoef = 0.05f;
            boxOne.Material.StaticFrictionCoef = 0.1f;
            //var boxOneGraphic = new SmoothedGraphicalBody(boxOne, FlatFactory.NewCuboid(3, 3, 0.5), 40);
            var boxOneGraphic = new BasicGraphicalBody(boxOne, FlatFactory.NewCuboid(3, 3, 0.5));

            var boxTwo = BodyFactory.Cuboid(3, 1, 0.5, new Vector3(3, 3, 5), Vector3.Zero, new Vector3(0.1, 0.1, 0.1));
            boxTwo.Material.Epsilon = 0.25f;
            boxTwo.Material.DynamicFrictionCoef = 0.05f;
            boxTwo.Material.StaticFrictionCoef = 0.1f;
            //var boxTwoGraphic = new SmoothedGraphicalBody(boxTwo, FlatFactory.NewCuboid(3, 3, 0.5), 40);
            var boxTwoGraphic = new BasicGraphicalBody(boxTwo, FlatFactory.NewCuboid(3, 1, 0.5));

            //var boxThree = BodyFactory.Cuboid(1, 4, 0.5, new Vector3(-3, -3, 2), new Vector3(2, 2, 0), new Vector3(3.1, 0.1, 9.1));
            //boxThree.Material.Epsilon = 0.25f;
            //boxThree.Material.DynamicFrictionCoef = 0.05f;
            //boxThree.Material.StaticFrictionCoef = 0.1f;
            ////var boxThreeGraphic = new SmoothedGraphicalBody(boxThree, FlatFactory.NewCuboid(1, 4, 0.5), 40);
            //var boxThreeGraphic = new BasicGraphicalBody(boxThree, FlatFactory.NewCuboid(1, 4, 0.5));

            var uni = new BasicUniverse();
            uni.ForceFields.Add(new ForceField(new ConstantVectorField(-1.0 * Vector3.K), ForceFieldFactory.GravityForceApplier));
            uni.Bodies.Add(floor);
            uni.Bodies.Add(boxOne, boxTwo);

            var vis = new OpenGLVisualization(new IGraphicalBody[] { floorGraphic, boxOneGraphic, boxTwoGraphic }, sun, light2);

            uni.ContactResolver = new LoopingContactResolver(new ImpulseCollisionEngine() { CollidingThresholdSpeed = 0.01f });
            uni.ContactFinder = new BasicContactFinder();

            //var physicsRunner = new RealTimePhysicsRunner(uni);
            var physicsRunner = new FixedTimePhysicsRunner(uni);
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Pause && inpt.State == InputState.Start).Subscribe(inpt => physicsRunner.TogglePause());
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Exit).Subscribe(inpt => vis.EndVis());
            vis.VisStarted += (sender, e) => physicsRunner.StartPhysics();

            var selector = InputHandlers.BodySelector(vis.Input, uni, vis.Camera);
            selector.Selection += (sender, e) => Console.WriteLine($"Selection Type: {e.SelectType}, Index: {uni.Bodies.ToList().IndexOf(e.Item)}, Total Selected: {selector.SelectedItems.Count}");

            return vis;
        }
    }
}
