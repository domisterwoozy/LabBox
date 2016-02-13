using Math3D.Geometry;
using Physics3D.Dynamics;
using Physics3D.ElectroMagnetism;
using Physics3D.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D
{
    public interface IBody
    {
        IDynamicBody Dynamics { get; }
        IVolume Shape { get; }
        Material Material { get; }
        EMProperties EMProps { get; }
    }
}
