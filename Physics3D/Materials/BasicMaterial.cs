using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Materials
{
    public class BasicMaterial : IMaterial
    {
        public double DragCoef { get; set; }

        public double DynamicFrictionCoef { get; set; }

        public double Epsilon { get; set; }

        public double StaticFrictionCoef { get; set; }
    }
}
