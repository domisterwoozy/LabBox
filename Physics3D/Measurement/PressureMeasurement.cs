using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics3D.Universes;
using Math3D.Geometry;
using Physics3D.Bodies;
using Math3D;

namespace Physics3D.Measurement
{
    public class PressureMeasurement : FrameProcessor<double>
    {
        private Dictionary<IBody, Vector3> initialMomentums;

        public IVolume Volume { get; }

        public PressureMeasurement(IVolume vol) : base("Impulse [momentum / time]")
        {
            Volume = vol;
        }

        protected override void ProcessBeforeFrame(IUniverse uni)
        {
            initialMomentums = uni.BodiesWithin(Volume).ToDictionary(b => b, b => b.Dynamics.P);
        }

        // the sum of the magnitude of the difference b/w each individual momentum
        protected override double ProcessAfterFrame(IUniverse uni) => initialMomentums.Sum(kv => (kv.Key.Dynamics.P - kv.Value).Magnitude);              
    }
}
