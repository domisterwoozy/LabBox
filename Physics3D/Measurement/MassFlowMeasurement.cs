using Math3D.Geometry;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    /// <summary>
    /// Measures the mass differential inside a specified volume.
    /// A positive result means there is mass flow into the volume and a negative result means there is mass flow out of the volume.
    /// </summary>
    public class MassFlowMeasurement : FrameProcessor<double>
    {
        private double massBefore = 0;

        public IVolume Volume { get; }

        public MassFlowMeasurement(IVolume vol) : base("Mass Flow [mass/area]")
        {
            Volume = vol;
        }

        protected override void ProcessBefore(IUniverse uni)
        {
            massBefore = uni.Bodies.Where(b => Volume.VolumeFunc(b.Dynamics.Transform.Pos)).Sum(b => b.Dynamics.Mass);
        }

        protected override double ProcessAfter(IUniverse uni)
        {
            double massAfter = uni.Bodies.Where(b => Volume.VolumeFunc(b.Dynamics.Transform.Pos)).Sum(b => b.Dynamics.Mass);
            return massAfter - massBefore;
        }        
    }


}
