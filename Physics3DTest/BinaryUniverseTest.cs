using Math3D;
using Math3D.VectorCalc;
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
    public class CircularBinaryUniverseTest
    {
        // inputs
        private static readonly double gravConstant = 1.0;
        private static readonly double massOne = 1.0;
        private static readonly double massTwo = 2.0; 
        private static readonly double distOne = 1.0; 
        private static readonly double distTwo = 1.5;
        private static readonly double speedOne = 0;
        private static readonly double speedTwo = 0;
        // derived
        private static readonly Vector3 posOne = distOne * Vector3.I;
        private static readonly Vector3 posTwo = -distTwo * Vector3.I;
        private static readonly Vector3 velOne = speedOne * Vector3.J;
        private static readonly Vector3 velTwo = -speedTwo * Vector3.J;
        // totals
        private static readonly Vector3 radiusVect = posOne + posTwo;
        private static readonly Vector3 totalVel = velOne + velTwo;
        private static readonly double totalMass = massOne + massTwo;
        private static readonly double totalAxis = distOne + distTwo;
        private static readonly double reducedMass = (massOne * massTwo) / (massOne + massTwo);
        private static readonly double period = Math.Sqrt(4 * Math.PI * Math.PI * Math.Pow(totalAxis, 3) / (gravConstant * totalMass));
        private static readonly double energy = 0.5;// * reducedMass * totalVel * totalVel - gravConstant * totalMass * reducedMass / radiusVect;



        public static IUniverse BinaryUniverse()
        {          
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
        public void CenterOfMassTest()
        {

        }
    }
}
