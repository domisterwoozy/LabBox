using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D;

namespace Physics3D.ElectroMagnetism
{
    public struct ChargeGroupMember
    {
        public double Charge { get; }
        public Vector3 Pos { get; }
        public Vector3 Vel { get; }

        public ChargeGroupMember(double charge, Vector3 pos, Vector3 vel)
        {
            Charge = charge;
            Pos = pos;
            Vel = vel;
        }
    }

    public class ChargeGroup : IElectroMag, IEnumerable<ChargeGroupMember>
    {
        private readonly List<ChargeGroupMember> charges;

        public double Charge => this.Sum(c => c.Charge);
        /// <summary>
        /// Summation of q * r.
        /// See Griffiths 3.100
        /// </summary>
        public Vector3 ElectricDipoleMoment => this.Sum(c => c.Charge * c.Pos);
        /// <summary>
        /// Summation of 0.5 * q * r X V
        /// See: https://en.wikipedia.org/wiki/Magnetic_moment#Integral_representation
        /// </summary>
        public Vector3 MagneticDipoleMoment => 0.5 * this.Sum(c => c.Charge * (c.Pos % c.Vel));

        public ChargeGroup(params ChargeGroupMember[] charges)
        {
            this.charges = charges.ToList();
        }

        public IEnumerator<ChargeGroupMember> GetEnumerator() => charges.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();     
    }
}
