using BasicVisualization;
using LabBox.Visualization.Universe;
using LabBox.Visualization.Universe.ViewModel;
using Math3D;
using OpenTK.Graphics;
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
            // lights
            var light = LightSource.Directional(new Vector3(1, 1, -1));
            light.CastsDynamicShadows = true;
            var light2 = LightSource.SpotLight(new Vector3(10, 0, 5), new Vector3(0, 0, -1), Math.PI / 4);
            light2.CastsDynamicShadows = true;
            var light3 = LightSource.PointLight(new Vector3(20, 20, -2), 100);
       
            // bodies
            var uni = SampleUniverses.SunEarth(10, 100);
            IEnumerable<IGraphicalBody> bodies = BasicGraphicalBody.FromPhysicsBodies(uni.Bodies);//.Select(b => b.NewShape(FlatFactory.NewCuboid(1, 1, 1))).Select(b => b.NewColor(Color.Lavender));
            MoveableBody floor = new MoveableBody(FlatFactory.NewWall(50, 50).NewColor(Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };            
            
            using (ILabBoxVis vis = new OpenGLVisualization(bodies.Union(floor), light, light2, light3))
            {
                var physicsRunner = new PausablePhysicsRunner(uni);
                vis.InputHandler.Pause.Start += (sender, e) => physicsRunner.TogglePause();
                vis.InputHandler.Exit.Start += (sender, e) => vis.EndVis();
                physicsRunner.StartPhysics();
                vis.RunVis();
            }
        }
    }
}
