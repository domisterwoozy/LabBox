using FakeItEasy;
using Math3D;
using Math3D.Geometry;
using MathTests;
using NUnit.Framework;
using Physics3D;
using Physics3D.Dynamics;
using Physics3D.Forces;
using Physics3D.Kinematics;
using Physics3D.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsTests
{
    public class ForceFieldTests
    {
        [Test]
        public void DragApplierTest([RandomVector3(2)]Vector3 windVect, [Random(0.1, 1.0, 2)]double cd, [Random(0.1, 10, 2)]double area, [RandomVector3(-10, 10, 2)]Vector3 vel)
        {
            var body = A.Fake<IBody>();
            var dyn = A.Fake<IDynamicBody>();
            var kin = A.Fake<IKinematics>();
            var shape = A.Fake<IVolume>();
            var mat = A.Fake<IMaterial>();

            A.CallTo(() => body.Dynamics).Returns(dyn);
            A.CallTo(() => body.Shape).Returns(shape);
            A.CallTo(() => body.Material).Returns(mat);
            A.CallTo(() => dyn.Kinematics).Returns(kin);
            A.CallTo(() => kin.V).Returns(vel);
            A.CallTo(() => shape.CrossSectionalArea(Vector3.Zero, vel)).Returns(area);
            A.CallTo(() => mat.DragCoef).Returns(cd);

            var coeff = vel.MagSquared * cd * area;
            Assert.That(ForceFields.DragForceApplier(body, windVect), Izz.EqualTo(coeff * windVect).Within(Math.Pow(10, -12)));
        }
    }
}
