using Math3D;
using Math3D.VectorCalc;
using Math3DTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physics3D;
using Physics3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3DTest
{
    [TestClass]
    public class UniverseTests
    {
        public static IUniverse SunEarthUniverse()
        {
            double gravConstant = 6.67408 * Math.Pow(10, -11);
            double solarMass = 1.98855 * Math.Pow(10, 30); // kg
            double earthMass = 5.972 * Math.Pow(10, 24); // kg
            double earthToSun = 149598023 * Math.Pow(10, 3); // meters
            double earthSpeed = 29.78 * Math.Pow(10, 3); // meters/s

            var earth = new RigidBody6DOF(
                new EuclideanKinematics(
                    new Transform(new Vector3(earthToSun, 0, 0), Matrix3.Identity),
                    new Vector3(0, earthSpeed, 0),
                    Vector3.Zero
                ),
                earthMass,
                Matrix3.Identity
            );

            var sunGravity = Potentials.InverseSquare(gravConstant * solarMass * earthMass); // GMm / r^2

            return new BasicUniverse
            {
                DynamicBodies = { earth },
                Potentials = { sunGravity }
            };
        }

        public static IUniverse SimpleUniverse()
        {
            double gravConstant = 1.0;
            double solarMass = 1.0; // kg
            double planetMass = 1.0; // kg
            double dist = 1.0; // meters
            double planetSpeed = Math.Sqrt((gravConstant * solarMass) / dist);

            var earth = new RigidBody6DOF(
                new EuclideanKinematics(
                    new Transform(new Vector3(dist, 0, 0), Matrix3.Identity),
                    new Vector3(0, planetSpeed, 0),
                    Vector3.Zero
                ),
                planetMass,
                Matrix3.Identity
            );

            var sunGravity = Potentials.InverseSquare(gravConstant * solarMass * planetMass); // GMm / r^2

            return new BasicUniverse
            {
                DynamicBodies = { earth },
                Potentials = { sunGravity }
            };
        }

        public static IUniverse BinaryUniverse()
        {
            double gravConstant = 1.0;
            double massOne = 1.0; // kg
            double massTwo = 2.0; // kg
            double distOne = 1.0; // meters
            double distTwo = 1.5;
            double speedOne = 0;
            double speedTwo = 0;
            Vector3 posOne = distOne * Vector3.I;
            Vector3 posTwo = -distTwo * Vector3.I;
            Vector3 velOne = speedOne * Vector3.J;
            Vector3 velTwo = -speedTwo * Vector3.J; 

            var bodyOne = new RigidBody6DOF(
                new EuclideanKinematics(
                    new Transform(posOne, Matrix3.Identity),
                    velOne,
                    Vector3.Zero
                ),
                massOne,
                Matrix3.Identity
            );

            var bodyTwo = new RigidBody6DOF(
                new EuclideanKinematics(
                    new Transform(posTwo, Matrix3.Identity),
                    velTwo,
                    Vector3.Zero
                ),
                massTwo,
                Matrix3.Identity
            );

            var grav = Potentials.InverseSquare(gravConstant * massOne * massTwo); // same underlying potential
            var gravOne = new TranslateableScalarField(grav, () => bodyOne.Transform.Pos);
            var gravTwo = new TranslateableScalarField(grav, () => bodyTwo.Transform.Pos);

            return new BasicUniverse
            {
                DynamicBodies = { bodyOne, bodyTwo },
                Potentials = { gravOne, gravTwo }
            };
        }

        [TestMethod]
        public void SunEarthPeriodTest()
        {
            const int numSteps = 10000;
            double tolerance = 5.2 * Math.Pow(10, 8);
            double halfYear = 3.1558149 * Math.Pow(10, 7) / 2.0; // seconds            
            double timeStep = halfYear / numSteps;
            

            IUniverse sunEarthUni = SunEarthUniverse();
            var earth = sunEarthUni.DynamicBodies.First();
            Vector3 earthStartPos = earth.Kinematics.Transform.Pos;
            Vector3 expectedHalfway = new Vector3(-earthStartPos.X, earthStartPos.Y, earthStartPos.Z);

            for (int i = 0; i < numSteps; i++)
            {
                sunEarthUni.Update(timeStep);
            }        
            Vector3TestsOld.VectorEqual(expectedHalfway, earth.Kinematics.Transform.Pos, tolerance);

            for (int i = 0; i < numSteps; i++)
            {
                sunEarthUni.Update(timeStep);
            }
            Vector3TestsOld.VectorEqual(earthStartPos, earth.Kinematics.Transform.Pos, tolerance);
        }

        [TestMethod]
        public void CircleOrbitPositionTest()
        {
            const int numSteps = 10000;
            double tolerance = 7 * Math.Pow(10, -4);
            double halfYear = Math.PI; // seconds            
            double timeStep = halfYear / numSteps;


            IUniverse uni = SimpleUniverse();
            var planet = uni.DynamicBodies.First();
            Vector3 startPos = planet.Kinematics.Transform.Pos;
            Vector3 expectedHalfway = new Vector3(-startPos.X, startPos.Y, startPos.Z);

            for (int i = 0; i < numSteps; i++)
            {
                uni.Update(timeStep);
            }
            Vector3TestsOld.VectorEqual(expectedHalfway, planet.Kinematics.Transform.Pos, tolerance);

            for (int i = 0; i < numSteps; i++)
            {
                uni.Update(timeStep);
            }
            Vector3TestsOld.VectorEqual(startPos, planet.Kinematics.Transform.Pos, tolerance);
        }

        [TestMethod]
        public void CircleOrbitEnergyTest()
        {
            const int numSteps = 10000;
            double tolerance = 3.2 * Math.Pow(10, -4);
            double year = 2 * Math.PI; // seconds            
            double timeStep = year / numSteps;


            IUniverse uni = SimpleUniverse();
            var planet = uni.DynamicBodies.First();
            Vector3 startPos = planet.Kinematics.Transform.Pos;

            double startingEnergy = planet.Energy;
            double maxEnergyDifference = 0;
            for (int i = 0; i < numSteps; i++)
            {
                uni.Update(timeStep);
                maxEnergyDifference = Math.Max(maxEnergyDifference, Math.Abs(startingEnergy - planet.Energy));
            }

            Assert.AreEqual(0, maxEnergyDifference, tolerance);
        }

        [TestMethod]
        public void CircleOrbitRadiusTest()
        {
            const int numSteps = 10000;
            double tolerance = 3.2 * Math.Pow(10, -4);
            double year = 2 * Math.PI; // seconds            
            double timeStep = year / numSteps;


            IUniverse uni = SimpleUniverse();
            var planet = uni.DynamicBodies.First();
            double expectedRadius = planet.Kinematics.Transform.Pos.Magnitude;

            double maxRadiusDifference = 0;
            for (int i = 0; i < numSteps; i++)
            {
                uni.Update(timeStep);
                maxRadiusDifference = Math.Max(maxRadiusDifference, Math.Abs(expectedRadius - planet.Kinematics.Transform.Pos.Magnitude));
            }

            Assert.AreEqual(0, maxRadiusDifference, tolerance);
        }

        public void BinaryCenterOfMassTest()
        {
            const int numSteps = 10000;
            double tolerance = 3.2 * Math.Pow(10, -4);
            double period = 4 * Math.Pow(Math.PI, 2) / (2);      
            double timeStep = period / numSteps;


            IUniverse uni = SimpleUniverse();
            var planet = uni.DynamicBodies.First();
            double expectedRadius = planet.Kinematics.Transform.Pos.Magnitude;

            double maxRadiusDifference = 0;
            for (int i = 0; i < numSteps; i++)
            {
                uni.Update(timeStep);
                maxRadiusDifference = Math.Max(maxRadiusDifference, Math.Abs(expectedRadius - planet.Kinematics.Transform.Pos.Magnitude));
            }

            Assert.AreEqual(0, maxRadiusDifference, tolerance);
        }


    }
}
