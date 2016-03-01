using Math3D;
using MathTests;
using NUnit.Framework;
using Physics3D;
using Physics3D.Kinematics;
using System;

namespace PhysicsTests
{
    public class EuclideanKinematicsTests
    {
        [Test]
        public void StraightLineTest([RandomVector3(3)]Vector3 omega, [Random(1.0, 10.0, 3)]double time)
        {
            var body = new EuclideanKinematicBody(Transform.Zero);
            body.Omega = omega;
            body.V = new Vector3(1, 1, 1);
            body.UpdateTransform(time);

            Assert.That(body.Transform.Pos, Is.EqualTo(new Vector3(time, time, time)));
        }

        public void SpinTest()
        {
            int numFrames = 1000;
            var body = new EuclideanKinematicBody(Transform.Zero);
            body.Omega = Vector3.K;
            body.UpdateTransform(1.0);

            for (int i = 0; i < numFrames; i++)
            {

            }

        }
    }
}
