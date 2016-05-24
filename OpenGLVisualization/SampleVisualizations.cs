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

            var floorGraphic = new MoveableBody(FlatFactory.NewCuboid(5.0, 1.0, 5.0).NewColor(Color.Gray)) { Translation = new Vector3(0, -0.5, 0) };

            var uni = SampleUniverses.SunEarth(0.75, 1.0, 0.75);
            var sunGraphic = new BasicGraphicalBody(uni.Bodies.First(), SphereFactory.NewSphere(0.4, 3)).NewColor(Color.DodgerBlue);
            var earthGraphic = new BasicGraphicalBody(uni.Bodies.Skip(1).First(), SphereFactory.NewSphere(0.2, 3));
            var moonGraphic = new BasicGraphicalBody(uni.Bodies.Skip(2).First(), SphereFactory.NewSphere(0.05, 3));

            var vis = new OpenGLVisualization(new IGraphicalBody[] { floorGraphic, sunGraphic, earthGraphic, moonGraphic }, true, sunLight, spotLight);

            var physicsRunner = new RealTimePhysicsRunner(uni);
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Pause && inpt.State == InputState.Start).Subscribe(inpt => physicsRunner.TogglePause());
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.SpeedUp && inpt.State == InputState.Start).Subscribe(inpt => physicsRunner.TimeMultiplier *= 2);
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.SlowDown && inpt.State == InputState.Start).Subscribe(inpt => physicsRunner.TimeMultiplier *= 0.5);
            vis.Input.InputEvents.Where(inpt => inpt.Input == InputType.Exit).Subscribe(inpt => vis.EndVis());
            vis.VisStarted += (sender, e) => physicsRunner.StartPhysics();

            return vis;
        }
    }
}
