using Math3D;
using Physics3D;
using Physics3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsIntegrationTests
{
    public static class SunEarthTest
    {
        private class SimResults
        {
            public long NumSteps { get; }
            public double HalfwayTime { get; } // seconds
            public double HalfwayDiff { get; } // meters
            public double FullTime { get; } // seconds
            public double FullDiff { get; } // meters

            public SimResults(long numSteps, double halfwayTime, double halfwayDiff, double fullTime, double fullDiff)
            {
                NumSteps = numSteps;
                HalfwayTime = halfwayTime;
                HalfwayDiff = halfwayDiff;
                FullTime = fullTime;
                FullDiff = fullDiff;
            }

            public static string ColumnHeaders() => "Num Steps,Halfway Time (s),Halfway Difference (m),Total Time (s),End Difference (m)";            
            public override string ToString() => $"{NumSteps},{HalfwayTime},{HalfwayDiff.ToString("e")},{FullTime},{FullDiff.ToString("e")}";            
        }

        public static string RunTest()
        {
            //long[] stepTrials = new[] { 100L, 1000L, 10000L };
            long[] stepTrials = new[] { 100L, 1000L, 10000L, 100000L, 1000000L };//, 10000000L, 1000000000L };

            StringBuilder output = new StringBuilder();
            output.AppendLine(SimResults.ColumnHeaders());
            foreach(long steps in stepTrials)
            {
                SimResults res = RunSim(steps);
                output.AppendLine(res.ToString());
            }
            return output.ToString();
        }

        private static IUniverse SunEarthUniverse()
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

        private static SimResults RunSim(long numSteps)
        {
            double halfYear = 3.1558149 * Math.Pow(10, 7) / 2.0; // seconds;
            double timeStep = halfYear / numSteps;

            IUniverse sunEarthUni = SunEarthUniverse();
            var earth = sunEarthUni.DynamicBodies.First();
            Vector3 earthStartPos = earth.Kinematics.Transform.Pos;
            Vector3 expectedHalfway = new Vector3(-earthStartPos.X, earthStartPos.Y, earthStartPos.Z);

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < numSteps; i++)
            {
                sunEarthUni.Update(timeStep);
            }
            stopwatch.Stop();
            double halfwayTime = stopwatch.Elapsed.TotalSeconds;
            double halfwayDiff =( expectedHalfway - earth.Kinematics.Transform.Pos).Magnitude;

            stopwatch.Start();
            for (int i = 0; i < numSteps; i++)
            {
                sunEarthUni.Update(timeStep);
            }
            stopwatch.Stop();
            double endTime = stopwatch.Elapsed.TotalSeconds;
            double endDiff = (earthStartPos - earth.Kinematics.Transform.Pos).Magnitude;

            return new SimResults(numSteps, halfwayTime, halfwayDiff, endTime, endDiff);
        }
    }
}
