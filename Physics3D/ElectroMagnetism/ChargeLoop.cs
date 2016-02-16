using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.ElectroMagnetism
{
    public class ChargeLoop : IElectroMag
    {
        public double Area { get; }
        public Vector3 Dir { get; }
        public double Current { get; }

        public ChargeLoop(double current, double area, Vector3 dir)
        {
            Area = area;
            Current = current;
            Dir = dir;
        }

        public double Charge => Current;
        public Vector3 ElectricDipoleMoment => Vector3.Zero;
        /// <summary>
        /// Griffiths 5.84
        /// </summary>
        public Vector3 MagneticDipoleMoment => Current * Area * Dir.UnitDirection;
    }
}
