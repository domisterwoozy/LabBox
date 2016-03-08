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
        public static BasicUniverse SunEarth(double distance, double sunMass)
        {
            double gravConstant = 1.0;
            double earthMass = Math.Pow(10, -5);

            double earthSpeed = Math.Sqrt(gravConstant * sunMass / distance); // for circular orbit

            var sun = BodyFactory.PointMass(sunMass, Vector3.Zero, Vector3.Zero);
            sun.Dynamics.Fix();
            var earth = BodyFactory.PointMass(earthMass, distance * Vector3.I, earthSpeed * Vector3.J);
            earth.Dynamics.ThrustInputs(Vector3.Zero, new Vector3(1,1,1), 1); // add a slight rotation to earth

            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, earth);
            uni.ForceFields.Add(ForceFieldFactory.Gravity(sun, gravConstant * sunMass));

            return uni;
        }

        public static BasicUniverse BouncyGravity(double distance, double sunMass)
        {
            double gravConstant = 1.0;
            double earthMass = Math.Pow(10, -5);
           
            var sun = BodyFactory.SphereMass(1.0, sunMass, Vector3.Zero, Vector3.Zero);
            sun.Dynamics.Fix();
            sun.Material.DynamicFrictionCoef = 0.0f;
            sun.Material.StaticFrictionCoef = 0.0f;

            var satOne = BodyFactory.SphereMass(1.0, earthMass, distance * Vector3.I, -Vector3.I + Vector3.J + Vector3.K);
            satOne.Material.Epsilon = 0.75f;
            satOne.Material.DynamicFrictionCoef = 0.0f;
            satOne.Material.StaticFrictionCoef = 0.0f;

            var satTwo = BodyFactory.SphereMass(1.0, earthMass, -distance * Vector3.I, Vector3.I - Vector3.J - Vector3.K);
            satTwo.Material.Epsilon = 0.75f;
            satTwo.Material.DynamicFrictionCoef = 0.0f;
            satTwo.Material.StaticFrictionCoef = 0.0f;

            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, satOne, satTwo);
            uni.ForceFields.Add(ForceFieldFactory.Gravity(sun, gravConstant * sunMass));

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
