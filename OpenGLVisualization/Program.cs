using BasicVisualization;
using LabBox.Visualization.Universe;
using LabBox.Visualization.Universe.ViewModel;
using Math3D;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Util;

namespace LabBox.OpenGLVisualization
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RunSim();
            //RunDebug();
        }

        private static void RunDebug()
        {
            using (var debug = new DebugVis())
            {
                debug.Run();
            }
        }

        private static void RunSim()
        {
            var light = LightSource.Directional(new Vector3(1, 1, -1));
            light.CastsDynamicShadows = true;
            var light2 = LightSource.SpotLight(new Vector3(10, 0, 5), new Vector3(0, 0, -1), Math.PI / 4);
            light2.CastsDynamicShadows = true;
            var light3 = LightSource.PointLight(new Vector3(20, 20, -2), 100.0f);
       
            var uni = SampleUniverses.SunEarth(10, 1000.0);
            IEnumerable<IGraphicalBody> bodies = BasicGraphicalBody.FromPhysicsBodies(uni.Bodies);
            MoveableBody floor = new MoveableBody(FlatFactory.NewCuboid(25, 25, 1, Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };

            
            var physicsRunner = new PausablePhysicsRunner(uni);
            using (ILabBoxVis vis = new OpenGLVisualization(bodies, light, light2, light3))
            {
                vis.InputHandler.Pause.Start += (sender, e) => physicsRunner.TogglePause();
                vis.InputHandler.Exit.Start += (sender, e) => vis.EndVis();
                physicsRunner.StartPhysics();
                Task.Run(() => WaitAndAddBody(vis, floor, 5000));
                Task.Run(() => WaitAndRemoveBody(vis, vis.Bodies.Skip(1).First(), 5000));
                vis.RunVis();
            }
        }

        private static void WaitAndAddBody(ILabBoxVis vis, IGraphicalBody body, int ms)
        {
            Thread.Sleep(ms);
            vis.AddBody(body);
        }

        private static void WaitAndRemoveBody(ILabBoxVis vis, IGraphicalBody body, int ms)
        {
            Thread.Sleep(ms);
            vis.RemoveBody(body);
            Task.Run(() => WaitAndAddBody(vis, body, 2000));
        }
    }
}
