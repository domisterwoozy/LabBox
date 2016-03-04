using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Materials
{
    public class BasicMaterial : IMaterial
    {
        public double DragCoef { get; set; } = 1.0f;

        public double DynamicFrictionCoef { get; set; } = 1.0f;

        public double Epsilon { get; set; } = 1.0f;

        public double StaticFrictionCoef { get; set; } = 1.0f;
    }
}
