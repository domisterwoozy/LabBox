using Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    public class PointLight
    {
        public Vector3 Position { get; set; }
        public float Power { get; set; }
        public Color LightColor { get; set; }
    }
}
