using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.ElectroMagnetism
{  
    public class Solenoid : IElectroMag
    {
        public int NumTurns { get; }
        public ChargeLoop Loop { get; }

        public double Charge => NumTurns * Loop.Charge;
        public Vector3 ElectricDipoleMoment => Vector3.Zero;
        public Vector3 MagneticDipoleMoment => NumTurns * Loop.MagneticDipoleMoment;
    }
}
