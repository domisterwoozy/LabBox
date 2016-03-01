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
using Physics3D.Kinematics;
using Math3D;

namespace Physics3D.Bodies
{
    public class BasicBody : IBody
    {
        public event EventHandler<FrameLengthArgs> FrameFinished;
        public IDynamicBody Dynamics { get; }
        public IElectroMag EMProps { get; }
        public IMaterial Material { get; }
        public IColliderVolume CollisionShape { get; }
        public IOverlapable BoundVolume { get; }

        public BasicBody(IDynamicBody dynamics, IElectroMag em, IMaterial mat, IColliderVolume shape, IOverlapable boundVolume)
        {
            Dynamics = dynamics;
            EMProps = em;
            Material = mat;
            CollisionShape = shape;
            BoundVolume = boundVolume;

            Dynamics.FrameFinished += (sender, e) => FrameFinished?.Invoke(sender, e);
        }        
    }
}
