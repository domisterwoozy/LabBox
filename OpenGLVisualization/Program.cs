using BasicVisualization;
using LabBox.Visualization.Universe;
using LabBox.Visualization.Universe.ViewModel;
using Math3D;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            var uni = SampleUniverses.SunEarth(10, 100.0);
            IEnumerable<IGraphicalBody> bodies = BasicGraphicalBody.FromPhysicsBodies(uni.Bodies, Color.Blue);
            MoveableBody floor = new MoveableBody(FlatFactory.NewWall(100, 100, Color.DodgerBlue)) { Translation = new Vector3(0, 0, -5) };

            var physicsRunner = new PausablePhysicsRunner(uni);
            using (ILabBoxVis vis = new OpenGLVisualization(floor.Union(bodies).ToArray()))
            {
                vis.InputHandler.Pause.Start += (sender, e) => physicsRunner.TogglePause();
                vis.InputHandler.Exit.Start += (sender, e) => vis.EndVis();
                physicsRunner.StartPhysics();
                vis.RunVis();
            }
        }
    }
}
