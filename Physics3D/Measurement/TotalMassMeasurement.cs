using Math3D.Geometry;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    public class TotalMassMeasurement : IInstantMeasurement<double>
    {
        public string Name => "Total Mass [mass]";
        public IVolume Volume { get; }

        public TotalMassMeasurement(IVolume vol)
        {
            Volume = vol;
        }

        public double TakeMeasurement(IUniverse uni) => uni.Bodies.Where(b => Volume.VolumeFunc(b.Dynamics.Transform.Pos)).Sum(b => b.Dynamics.Mass);
    }
}
