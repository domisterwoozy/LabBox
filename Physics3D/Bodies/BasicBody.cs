using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Geometry;
using Physics3D.Dynamics;
using Physics3D.ElectroMagnetism;
using Physics3D.Materials;

namespace Physics3D.Bodies
{
    public class BasicBody : IBody
    {
        public IDynamicBody Dynamics { get; }
        public IElectroMag EMProps { get; }
        public IMaterial Material { get; }
        public IVolume Shape { get; }

        public BasicBody(IDynamicBody dynamics, IElectroMag em, IMaterial mat, IVolume shape)
        {
            Dynamics = dynamics;
            EMProps = em;
            Material = mat;
            Shape = shape;
        }
    }
}
