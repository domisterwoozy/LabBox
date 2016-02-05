using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics3D;
using Math3D;
using Math3DTests;

namespace Physics3DTest
{
    [TestClass]
    public class EuclideanKinematicTests
    {
        [TestMethod]
        public void UpwardLaunchTest()
        {
            const int framePerSecond = 1000;
            double timeStep = 1.0 / framePerSecond;

            var ekBody = new EuclideanKinematicBody(Transform.Zero);
            ekBody.V = Vector3.K; // up and 1 unit/s

            double elapsedTime = 0;
            while (ekBody.Transform.Pos.Z < (10.0 - MathConstants.DoubleEqualityTolerance))
            {
                ekBody.UpdateTransform(timeStep);
                elapsedTime += timeStep;
            }

            double expected = 10.0;
            Assert.AreEqual(expected, elapsedTime, MathConstants.DoubleEqualityTolerance);
        }

        [TestMethod]
        public void SpinTest()
        {
            const int totalSteps = 1000000;
            double compTolerance = Math.Pow(10, -5);
            const double totalTime = 1.0;
            double timeStep = totalTime / totalSteps;

            var ekBody = new EuclideanKinematicBody(Transform.Zero);
            ekBody.Omega = Vector3.K; // around z axis (1 rads/s)

            for (int i = 0; i < totalSteps; i++)
            {
                ekBody.UpdateTransform(timeStep);
            }

            double totalRotRads = totalTime; // total angular rotation in radians
            Vector3 expectedI = Vector3.I.Rotate(Quaternion.UnitQuaternion(totalRotRads, Vector3.K)); 
            Vector3 expectedJ = Vector3.J.Rotate(Quaternion.UnitQuaternion(totalRotRads, Vector3.K));

            Vector3TestsOld.VectorEqual(Vector3.K, ekBody.Transform.K, MathConstants.DoubleEqualityTolerance); // body up and world up should still be aligned
            Vector3TestsOld.VectorEqual(expectedI, ekBody.Transform.I, compTolerance);
            Vector3TestsOld.VectorEqual(expectedJ, ekBody.Transform.J, compTolerance);
        }

        [TestMethod]
        public void UpwardLaunchWithGravityTest()
        {
            const int totalSteps = 1000000;
            double compTolerance = Math.Pow(10, -5);
            const double totalTime = 1.0;
            double timeStep = totalTime / totalSteps;

            var body = new RigidBody6DOF(new EuclideanKinematics(Transform.Zero, Vector3.K, Vector3.Zero), 1.0, Matrix3.Identity);
            body.AddInputs(new Vector3(0, 0, -9.8), Vector3.Zero);  // gravity          

            for (int i = 0; i < totalSteps; i++)
            {
                body.Update(timeStep);
            }

            Vector3 expectedVelocity = new Vector3(0, 0, -8.8);
            Vector3 expectedPosition = new Vector3(0, 0, -3.9);
            Vector3TestsOld.VectorEqual(expectedVelocity, body.Kinematics.V, compTolerance);
            Vector3TestsOld.VectorEqual(expectedPosition, body.Kinematics.Transform.Pos, compTolerance);

            Vector3TestsOld.VectorEqual(Vector3.Zero, body.Kinematics.Omega, MathConstants.DoubleEqualityTolerance);
            Assert.AreEqual(Matrix3.Identity, body.Kinematics.Transform.R);
        }
    }
}
