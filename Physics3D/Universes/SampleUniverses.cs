using Math3D;
using Math3D.Geometry;
using Math3D.VectorCalc;
using Physics3D.Bodies;
using Physics3D.Collisions;
using Physics3D.Forces;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Physics3D.Universes
{
    public static class SampleUniverses
    {
        public static BasicUniverse SunEarth(double distance, double sunMass, double height = 0)
        {
            double gravConstant = 1.0;
            double earthMass = Math.Pow(10, -5);
            double moonMass = Math.Pow(10, -10);

            double earthSpeed = Math.Sqrt(gravConstant * sunMass / distance); // for circular orbit
            earthSpeed *= 1.2;

            double moonSpeed = Math.Sqrt(gravConstant * sunMass / (0.8 * distance));

            var sun = BodyFactory.PointMass(sunMass, Vector3.Zero + Vector3.J * height, Vector3.Zero);
            sun.Dynamics.Fix();
            var earth = BodyFactory.PointMass(earthMass, distance * Vector3.I + Vector3.J * height, earthSpeed * Vector3.K);
            earth.Dynamics.ThrustInputs(Vector3.Zero, new Vector3(0.1,0.1,0.1), 1); // add a slight rotation to earth
            var moon = BodyFactory.PointMass(moonMass, 0.8 * distance * Vector3.I + Vector3.J * height, moonSpeed * Vector3.J);
            moon.Dynamics.ThrustInputs(Vector3.Zero, new Vector3(0.1, 0.1, 0.1), 1); // add a slight rotation to earth

            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, earth, moon);
            uni.ForceFields.Add(ForceFieldFactory.Gravity(sun, gravConstant * sunMass));

            return uni;
        }

        public static BasicUniverse BouncyGravity(double distance, double sunMass)
        {
            double gravConstant = 1.0;
            double earthMass = Math.Pow(10, -5);
           
            var sun = BodyFactory.SphereMass(0.2, sunMass, Vector3.Zero, Vector3.Zero);
            sun.Dynamics.Fix();
            sun.Material.DynamicFrictionCoef = 0.0f;
            sun.Material.StaticFrictionCoef = 0.0f;

            var satOne = BodyFactory.SphereMass(0.2, earthMass, distance * Vector3.I, Vector3.Zero);
            satOne.Material.Epsilon = 0.75f;
            satOne.Material.DynamicFrictionCoef = 0.05f;
            satOne.Material.StaticFrictionCoef = 0.1f;

            var satTwo = BodyFactory.SphereMass(0.2, earthMass, -distance * Vector3.I, Vector3.I - Vector3.J - Vector3.K);
            satTwo.Material.Epsilon = 0.75f;
            satTwo.Material.DynamicFrictionCoef = 0.05f;
            satTwo.Material.StaticFrictionCoef = 0.1f;

            var satThree = BodyFactory.SphereMass(0.2, earthMass, distance * Vector3.J, Vector3.I - Vector3.J - Vector3.K);
            satThree.Material.Epsilon = 0.75f;
            satThree.Material.DynamicFrictionCoef = 0.05f;
            satThree.Material.StaticFrictionCoef = 0.1f;

            var satFour = BodyFactory.SphereMass(0.2, earthMass, -distance * Vector3.J, Vector3.I - Vector3.J - Vector3.K);
            satFour.Material.Epsilon = 0.75f;
            satFour.Material.DynamicFrictionCoef = 0.05f;
            satFour.Material.StaticFrictionCoef = 0.1f;

            var satFive = BodyFactory.SphereMass(0.2, 10 * earthMass, -10 *distance * Vector3.J, Vector3.J);
            satFive.Material.Epsilon = 0.75f;
            satFive.Material.DynamicFrictionCoef = 0.05f;
            satFive.Material.StaticFrictionCoef = 0.1f;

            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, satOne, satTwo, satThree, satFour, satFive);
            uni.ForceFields.Add(ForceFieldFactory.Gravity(sun, gravConstant * sunMass));

            return uni;
        }

        public static BasicUniverse BouncyBalls(double distance, double speed)
        {
            var satOne = BodyFactory.SphereMass(1.0, 1.0, distance * Vector3.I, speed * (-Vector3.I + Vector3.J + Vector3.K));
            satOne.Material.Epsilon = 0.75;
            satOne.Material.DynamicFrictionCoef = 0.5f;
            satOne.Material.StaticFrictionCoef = 1.0f;

            var satTwo = BodyFactory.SphereMass(1.0, 1.0, -distance * Vector3.I, speed * (Vector3.I - Vector3.J - Vector3.K));
            satTwo.Material.Epsilon = 1.0f;
            satTwo.Material.DynamicFrictionCoef = 0.05f;
            satTwo.Material.StaticFrictionCoef = 0.1f;

            var satThree = BodyFactory.SphereMass(1.0, 1.0, distance * Vector3.J, speed * (Vector3.I - Vector3.J - Vector3.K));
            satThree.Material.Epsilon = 1.0f;
            satThree.Material.DynamicFrictionCoef = 0.05f;
            satThree.Material.StaticFrictionCoef = 0.1f;

            var satFour = BodyFactory.SphereMass(1.0, 1.0, -distance * Vector3.J, speed * (Vector3.I - Vector3.J - Vector3.K));
            satFour.Material.Epsilon = 1.0f;
            satFour.Material.DynamicFrictionCoef = 0.05f;
            satFour.Material.StaticFrictionCoef = 0.1f;

            var uni = new BasicUniverse();
            uni.Bodies.Add( satOne, satTwo, satThree, satFour);
            uni.ForceFields.Add(new ForceField(new ConstantVectorField(-9.8 * Vector3.K), ForceFieldFactory.GravityForceApplier));

            return uni;
        }

        public static BasicUniverse DirectBounce(double distance, double sunMass)
        {
            double earthMass = Math.Pow(10, -5);

            var sun = BodyFactory.SphereMass(1.0, sunMass, Vector3.Zero, Vector3.Zero);
            sun.Dynamics.Fix();
            sun.Material.Epsilon = 0.5f;

            var satOne = BodyFactory.SphereMass(1.0, earthMass, distance * Vector3.K, Vector3.Zero);
            satOne.Material.Epsilon = 0.5f;


            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, satOne);
            uni.ForceFields.Add(new ForceField(new ConstantVectorField(-9.8*Vector3.K), ForceFieldFactory.GravityForceApplier));

            return uni;
        }

        public static BasicUniverse BoxOfStuff()
        {
            IEnumerable<IBody> box = BodyFactory.Box(30, 30, 10, Vector3.Zero);

            var ballOne = BodyFactory.SphereMass(1, 1, new Vector3(2,2,2), new Vector3(1.1, -2, 4) * 5);

            var uni = new BasicUniverse();
            uni.Bodies.Add(box.ToArray());
            uni.Bodies.Add(ballOne);
            //uni.ForceFields.Add(new ForceField(new ConstantVectorField(-9.8*Vector3.K), ForceFieldFactory.GravityForceApplier));

            return uni;
        }
    }
}
