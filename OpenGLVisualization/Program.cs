using BasicVisualization;
using LabBox.Visualization.Universe;
using LabBox.Visualization.Universe.ViewModel;
using Math3D;
using OpenTK.Graphics;
using Physics3D.Collisions;
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
            using (ILabBoxVis vis = SampleVisualizations.BouncyBalls())
            {
                vis.RunVis();
            }
        }
    }
}
