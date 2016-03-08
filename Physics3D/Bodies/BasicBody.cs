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
        public BasicMaterial Material { get; }
        public IEdgeIntersector CollisionShape { get; }
        public IVolume Shape { get; }
        public IOverlapable BoundVolume { get; }

        IMaterial IBody.Material => Material;
        
        public BasicBody(IDynamicBody dynamics, IElectroMag em, BasicMaterial mat, IEdgeIntersector intersectionShape, IOverlapable boundVolume, IVolume shape)
        {
            Dynamics = dynamics;
            EMProps = em;
            Material = mat;
            CollisionShape = intersectionShape;
            BoundVolume = boundVolume;
            Shape = shape;

            Dynamics.FrameFinished += (sender, e) => FrameFinished?.Invoke(sender, e);
        }        
    }
}
