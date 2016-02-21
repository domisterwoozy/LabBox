using BasicVisualization.Implementations.OpenGL;
using BasicVisualization.Universe.ViewModel;
using Math3D;
using System;

namespace BasicVisualization
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var b = new BasicGraphicalBody(GraphicsFactory.NewCuboid(2, 1, 1));
            var b1 = new BasicGraphicalBody(GraphicsFactory.NewCuboid(1, 1, 1));


            using (ILabBoxVis vis = new BasicVis(b, b1))
            {
                vis.RunVis();
            }
        }
    }   
}
