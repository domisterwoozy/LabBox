using System;
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
        public BasicMaterial Material { get; }
        public IEdgeIntersector CollisionShape { get; }
        public IVolume Shape { get; }
        public IOverlapable BoundVolume { get; }

        IMaterial IBody.Material => Material;

        public BasicBody(IDynamicBody dynamics, IElectroMag eMProps, BasicMaterial material, IEdgeIntersector collisionShape, IVolume shape, IOverlapable boundVolume)
        {
            Dynamics = dynamics;
            EMProps = eMProps;
            Material = material;
            CollisionShape = collisionShape;
            Shape = shape;
            BoundVolume = boundVolume;

            Dynamics.FrameFinished += (sender, e) => FrameFinished?.Invoke(sender, e);
        }

        public class Builder
        {
            public IElectroMag EMProps { get; set; }
            public BasicMaterial Material { get; set; }
            public IVolume Shape { get; set; }
            public IOverlapable BoundVolume { get; set; }
            public IEdgeIntersector CollisionShape { get; set; }
            public IDynamicBody Dynamics { get; set; }

            public BasicBody Build()
            {
                return new BasicBody(Dynamics, EMProps, Material, CollisionShape, Shape, BoundVolume);
            }
        }

        public BasicBody WithMaterial(BasicMaterial newMaterial) => new BasicBody(Dynamics, EMProps, newMaterial, CollisionShape, Shape, BoundVolume);
        public BasicBody WithShape(IVolume newShape) => new BasicBody(Dynamics, EMProps, Material, CollisionShape, newShape, BoundVolume);
        public BasicBody WithBoundVolume(IOverlapable newBoundVolume) => new BasicBody(Dynamics, EMProps, Material, CollisionShape, Shape, newBoundVolume);
        public BasicBody WithCollisionShape(IEdgeIntersector newCollisionShape) => new BasicBody(Dynamics, EMProps, Material, newCollisionShape, Shape, BoundVolume);
        public BasicBody WithDynamics(IDynamicBody newDynamics) => new BasicBody(newDynamics, EMProps, Material, CollisionShape, Shape, BoundVolume);
        public BasicBody WithEMProps(IElectroMag newEMProps) => new BasicBody(Dynamics, newEMProps, Material, CollisionShape, Shape, BoundVolume);
    }
}
