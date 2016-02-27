using BasicVisualization.Implementations.OpenGL;
using BasicVisualization.Input;
using BasicVisualization.Universe;
using BasicVisualization.Universe.ViewModel;
using Math3D;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Util;

namespace BasicVisualization
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var uni = SampleUniverses.SunEarth(10, 100.0);
            IEnumerable<IGraphicalBody> bodies = BasicGraphicalBody.FromPhysicsBodies(uni.Bodies);
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
