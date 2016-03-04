using Math3D;
using Math3D.Geometry;
using Physics3D.Bodies;
using Physics3D.Forces;
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
            var earth = BodyFactory.PointMass(earthMass, distance * Vector3.I, earthSpeed * Vector3.J);
            earth.Dynamics.ThrustInputs(Vector3.Zero, new Vector3(1,1,1), 1); // add a slight rotation to earth

            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, earth);
            uni.ForceFields.Add(ForceFieldFactory.Gravity(sun, gravConstant));

            return uni;
        }

        public static BasicUniverse BouncyGravity(double distance, double sunMass)
        {
            double gravConstant = 1.0;
            double earthMass = Math.Pow(10, -5);

            double earthSpeed = 1.0; // constant speed

            var sun = BodyFactory.SphereMass(1.0, sunMass, Vector3.Zero, Vector3.Zero);
            sun.Dynamics.Fix();
            var earth = BodyFactory.SphereMass(1.0, earthMass, distance * Vector3.I, -Vector3.I + Vector3.J);

            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, earth);
            uni.ForceFields.Add(ForceFieldFactory.Gravity(sun, gravConstant * sunMass));

            return uni;
        }
    }
}
