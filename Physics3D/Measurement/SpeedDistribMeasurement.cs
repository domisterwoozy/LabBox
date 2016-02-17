using Math3D.Probability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics3D.Universes;
using Math3D.Geometry;

namespace Physics3D.Measurement
{
    public class SpeedDistribMeasurement : IInstantMeasurement<ContinuousDistribution>
    {
        public string Name => "Velocity Distribution";

        public IVolume Volume { get; }

        public SpeedDistribMeasurement(IVolume vol)
        {
            Volume = vol;
        }

        public ContinuousDistribution TakeMeasurement(IUniverse uni) => new ContinuousDistribution(uni.BodiesWithin(Volume).Select(b => b.Dynamics.Kinematics.V.Magnitude));
    }
}
