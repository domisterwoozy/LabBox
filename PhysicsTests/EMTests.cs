using Math3D;
using MathTests;
using NUnit.Framework;
using Physics3D.ElectroMagnetism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsTests
{
    public class EMTests
    {
        [Test]
        public void PointChargeTest([RandomDouble(5)]double charge)
        {
            var pointCharge = new PointCharge(charge);
            Assert.That(pointCharge.ElectricDipoleMoment, Is.EqualTo(Vector3.Zero));
            Assert.That(pointCharge.MagneticDipoleMoment, Is.EqualTo(Vector3.Zero));
            Assert.That(pointCharge.Charge, Is.EqualTo(charge));
        }
    }
}
