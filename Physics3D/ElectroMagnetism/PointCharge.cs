using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D;

namespace Physics3D.ElectroMagnetism
{
    /// <summary>
    /// A stationary point charge.
    /// </summary>
    public class PointCharge : IElectroMag
    {
        public double Charge { get; }
        public Vector3 MagneticDipoleMoment => Vector3.Zero;
        public Vector3 ElectricDipoleMoment => Vector3.Zero;

        public PointCharge(double charge)
        {
            Charge = charge;
        }
    }
}
