using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            BasicGraphicalBody b = ShapeFactory.newCuboid(2, 1, 1);
            BasicGraphicalBody b1 = ShapeFactory.newCuboid(1, 1, 1);
            b1.Translation = new Math3D.Vector3(0, 5, 0);
            //b.Orientation = Math3D.Quaternion.UnitQuaternion(Math.PI / 4, Math3D.Vector3.K);
            using (var win = new BasicVis(b, b1))
            {
                win.Run();
            }
        }
    }   
}
