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
using System.Collections.Immutable;
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

        public static OpenGLVisualization TwoBody()
        {
            var sunLight = LightSource.Directional(new Vector3(1, -1, 1));
            sunLight.CastsDynamicShadows = true;

            var spotLight = LightSource.SpotLight(new Vector3(-1.5, 5, -1.5), new Vector3(0, -1, 0), Math.PI / 16);
            spotLight.CastsDynamicShadows = true;

            var floorGraphic = new MoveableBody(FlatFactory.NewCuboid(5, 1.0, 5).NewColor(Color.White)) { Translation = new Vector3(0, -0.5, 0) };

            var uni = SampleUniverses.SunEarth(1.5, 1.0, 0.5);
            var sunGraphic = new BasicGraphicalBody(uni.Bodies.First(), SphereFactory.NewSphere(0.5, 3)).NewColor(Color.Yellow);
            var earthGraphic = new BasicGraphicalBody(uni.Bodies.Skip(1).First(), SphereFactory.NewSphere(0.2, 3)).NewColor(Color.DodgerBlue);

            var vis = new OpenGLVisualization(new IGraphicalBody[] { floorGraphic, sunGraphic, earthGraphic }, sunLight, spotLight);

            var physicsRunner = new RealTimePhysicsRunner(uni);
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Pause && inpt.State == InputState.Start).Subscribe(inpt => physicsRunner.TogglePause());
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Exit).Subscribe(inpt => vis.EndVis());
            vis.VisStarted += (sender, e) => physicsRunner.StartPhysics();

            return vis;
        }

        public static OpenGLVisualization BouncyGravity()
        {
            var sun = LightSource.Directional(new Vector3(1, 1, -1));
            sun.CastsDynamicShadows = true;
            var light2 = LightSource.SpotLight(new Vector3(10, 0, 5), new Vector3(0, 0, -1), Math.PI / 4);
            light2.CastsDynamicShadows = true;

            //IEnumerable<IBody> box = BodyFactory.Box(30, 30, 10, Vector3.Zero);
            var floorGraphic = new MoveableBody(FlatFactory.NewCuboid(10, 10, 1.0).NewColor(Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };

            //var balls = new List<IBody>();
            //var ball = BodyFactory.SphereMass(1, 1, new Vector3(2, 2, 2), new Vector3(1.1, -2, 4));
            //ball.Material.Epsilon = 0.5f;
            //balls.Add(ball);      

            var uni = SampleUniverses.BouncyGravity(10, 1000);
            //uni.ForceFields.Add(new ForceField(new ConstantVectorField(-9.8 * Vector3.K), ForceFieldFactory.GravityForceApplier));
            var bodyGraphics = uni.Bodies.Take(5).Select(b => new BasicGraphicalBody(b, SphereFactory.NewSphere(0.2, 3)));
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
            var floorGraphic = new BasicGraphicalBody(floor, FlatFactory.NewCuboid(30, 30, 1.0).NewColor(Color.DodgerBlue).ToImmutableArray());

            var boxOne = BodyFactory.Cuboid(3, 3, 0.5, new Vector3(3, 3, 2), Vector3.Zero, new Vector3(0.1, 0.1, 0.1));
            boxOne.Material.Epsilon = 0.25f;
            boxOne.Material.DynamicFrictionCoef = 0.05f;
            boxOne.Material.StaticFrictionCoef = 0.1f;
            var boxOneGraphic = new BasicGraphicalBody(boxOne, FlatFactory.NewCuboid(3, 3, 0.5).NewColor(Color.DodgerBlue).ToImmutableArray());

            var boxTwo = BodyFactory.Cuboid(3, 1, 0.5, new Vector3(3, 3, 5), Vector3.Zero, new Vector3(0.1, 0.1, 0.1));
            boxTwo.Material.Epsilon = 0.25f;
            boxTwo.Material.DynamicFrictionCoef = 0.05f;
            boxTwo.Material.StaticFrictionCoef = 0.1f;
            var boxTwoGraphic = new BasicGraphicalBody(boxTwo, FlatFactory.NewCuboid(3, 1, 0.5).NewColor(Color.DodgerBlue).ToImmutableArray());

            var uni = new BasicUniverse();
            uni.ForceFields.Add(new ForceField(new ConstantVectorField(-1.0 * Vector3.K), ForceFieldFactory.GravityForceApplier));
            uni.Bodies.Add(floor);
            uni.Bodies.Add(boxOne, boxTwo);

            var bodyToGraphics = new Dictionary<IBody, IGraphicalBody>
            {
                {floor, floorGraphic },
                {boxOne, boxOneGraphic },
                {boxTwo, boxTwoGraphic }
            };
            var vis = new OpenGLVisualization(bodyToGraphics.Values, sun, light2);

            uni.ContactResolver = new LoopingContactResolver(new ImpulseCollisionEngine() { CollidingThresholdSpeed = 0.01f });
            uni.ContactFinder = new BasicContactFinder();

            //var physicsRunner = new RealTimePhysicsRunner(uni);
            var physicsRunner = new FixedTimePhysicsRunner(uni);
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Pause && inpt.State == InputState.Start).Subscribe(inpt => physicsRunner.TogglePause());
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Exit).Subscribe(inpt => vis.EndVis());
            vis.VisStarted += (sender, e) => physicsRunner.StartPhysics();


            var selector = InputHandlers.BodySelector(vis.Input, uni, vis.Camera);
            selector.Selection += (sender, e) =>
            {
                IGraphicalBody oldGraphic = bodyToGraphics[e.Item];
                IGraphicalBody newGraphic;
                if (e.SelectType == SelectionType.Selected)
                {
                    // turn green
                    newGraphic = oldGraphic.NewColor(Color.Green);
                }
                else
                {
                    // turn random
                    var rand = new Random();
                    newGraphic = oldGraphic.NewColor(Color.FromArgb(255, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
                }
                bodyToGraphics[e.Item] = newGraphic;
                vis.RemoveBody(oldGraphic);
                vis.AddBody(newGraphic);
            };

            return vis;
        }
    }
}
