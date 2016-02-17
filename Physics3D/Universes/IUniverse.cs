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
        ICollisionEngine CollisionEngine { get; }
        void Update(double deltaTime);
    }

    public static class UniverseExtensions
    {
        public static IEnumerable<IBody> BodiesWithin(this IUniverse uni, IVolume vol) => uni.Bodies.Where(b => vol.VolumeFunc(b.Dynamics.Transform.Pos));
    }
}
