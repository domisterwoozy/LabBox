using Math3D;
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
        public static IUniverse SunEarth(double distance, double sunMass)
        {
            double gravConstant = 1.0;
            double earthMass = Math.Pow(10, -5);

            double earthSpeed = Math.Sqrt(gravConstant * sunMass / distance); // for circular orbit

            var sun = BodyFactory.PointMass(sunMass, Vector3.Zero, Vector3.Zero);
            var earth = BodyFactory.PointMass(earthMass, distance * Vector3.I, earthSpeed * Vector3.J);

            var uni = new BasicUniverse();
            uni.Bodies.Add(sun, earth);
            uni.ForceFields.Add(ForceFieldFactory.Gravity(sun, gravConstant));

            return uni;
        }
    }
}
