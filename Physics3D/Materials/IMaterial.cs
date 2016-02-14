using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Materials
{
    public interface IMaterial
    {
        double DynamicFrictionCoef { get; }
        double StaticFrictionCoef { get; }
        double DragCoef { get; }
    }
}
