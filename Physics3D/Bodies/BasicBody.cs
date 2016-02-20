using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Geometry;
using Physics3D.Dynamics;
using Physics3D.ElectroMagnetism;
using Physics3D.Materials;
using Physics3D.Universes;

namespace Physics3D.Bodies
{
    public class BasicBody : IBody
    {
        public event EventHandler<FrameLengthArgs> FrameFinished;
        public IDynamicBody Dynamics { get; }
        public IElectroMag EMProps { get; }
        public IMaterial Material { get; }
        public IPrimitiveVolume CollisionShape { get; }

        public BasicBody(IDynamicBody dynamics, IElectroMag em, IMaterial mat, IPrimitiveVolume shape)
        {
            Dynamics = dynamics;
            EMProps = em;
            Material = mat;
            CollisionShape = shape;

            Dynamics.FrameFinished += (sender, e) => FrameFinished?.Invoke(sender, e);
        }
    }
}
