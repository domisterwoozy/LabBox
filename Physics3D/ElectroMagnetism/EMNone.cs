using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D;

namespace Physics3D.ElectroMagnetism
{
    public class EMNone : IElectroMag
    {
        public static readonly EMNone Instance = new EMNone();

        public double Charge => 0;
        public Vector3 ElectricDipoleMoment => Vector3.Zero;
        public Vector3 MagneticDipoleMoment => Vector3.Zero;

        private EMNone() { }
    }
}
