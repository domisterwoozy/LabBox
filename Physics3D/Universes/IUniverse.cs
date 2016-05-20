using Math3D;
using Math3D.Geometry;
using Math3D.VectorCalc;
using Physics3D.Bodies;
using Physics3D.Collisions;
using Physics3D.Dynamics;
using Physics3D.Forces;
using Physics3D.Measurement;
using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Physics3D.Universes
{
    public class FrameLengthArgs : EventArgs
    {
        public double FrameLength { get; }
        public FrameLengthArgs(double deltaTime)
        {
            FrameLength = deltaTime;
        }
    }

    public interface IUniverse
    {
        event EventHandler<FrameLengthArgs> FrameFinished;

        double UniversalTime { get; }

        ICollection<IBody> Bodies { get; }
        ICollection<ForceField> ForceFields { get; }
        IContactResolver ContactResolver { get; }

        void Update(double deltaTime);
    }

    public static class UniverseExtensions
    {
        public static IEnumerable<IBody> BodiesWithin(this IUniverse uni, IVolume vol) => uni.Bodies.Where(b => vol.VolumeFunc(b.Dynamics.Transform.Pos));

        public static Optional<IBody> RaySelect(this IUniverse uni, Vector3 origin, Vector3 dir, IRay rayCastProvider = null)
        {
            if (rayCastProvider == null) rayCastProvider = new Ray();
            double furthestBodyPos = uni.Bodies.Max(b => (b.Position() - origin).Magnitude) + 100; // need to tune this. need to basically find the max dimension size of the biggest object
            var res = rayCastProvider.Cast(uni.Bodies.Select(b => new TransformedObj<IEdgeIntersector>(b.Dynamics.Transform, b.CollisionShape)), origin, dir, furthestBodyPos);
            return
                res.Match(
                    hit => uni.Bodies.Single(b => b.CollisionShape == hit.HitObject.Obj).ToOptional(),
                    () => Optional<IBody>.Nothing
                );
        }
    }
}
