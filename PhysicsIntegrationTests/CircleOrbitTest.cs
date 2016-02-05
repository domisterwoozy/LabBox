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
    public static class CircleOrbitTest
    {
        public static string Name = "CircleOrbitTest";
        private class SimResults
        {
            public long NumSteps { get; }
            public double HalfwayDiff { get; } // meters
            public double FullTime { get; } // seconds
            public double FullDiff { get; } // meters
            public double EnergyFluctuation { get; }
            public double RadiusFluctuation { get; }

            public SimResults(long numSteps, double endTime, double halfwayDiff, double fullDiff, double energyFluc, double radiusFluc)
            {
                NumSteps = numSteps;
                HalfwayDiff = halfwayDiff;
                FullTime = endTime;
                FullDiff = fullDiff;
                EnergyFluctuation = energyFluc;
                RadiusFluctuation = radiusFluc;
            }

            public static string ColumnHeaders() => "Num Steps,Total Time (s),Halfway Difference (ratio),End Difference (ratio),Energy Fluctuation (ratio), Radius Fluctuation (ratio)";
            public override string ToString() => 
                $"{NumSteps.ToString("e")},{FullTime},{HalfwayDiff.ToString("e")},{FullDiff.ToString("e")}," +
                $"{EnergyFluctuation.ToString("e")},{RadiusFluctuation.ToString("e")}";
        }

        public static string RunTest()
        {
            //long[] stepTrials = new[] { 100L, 1000L, 10000L };
            long[] stepTrials = new[] { 100L, 1000L, 10000L, 100000L, 1000000L };//, 10000000L, 1000000000L };

            StringBuilder output = new StringBuilder();
            output.AppendLine(SimResults.ColumnHeaders());
            foreach (long steps in stepTrials)
            {
                SimResults res = RunSim(steps);
                output.AppendLine(res.ToString());
            }
            return output.ToString();
        }

        private static IUniverse SimpleUniverse()
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

        private static SimResults RunSim(long halfNumSteps)
        {
            double halfYear = Math.PI; // seconds;
            double timeStep = halfYear / halfNumSteps;

            IUniverse sunEarthUni = SimpleUniverse();
            var planet = sunEarthUni.DynamicBodies.First();
            Vector3 planetStartPos = planet.Kinematics.Transform.Pos;
            Vector3 expectedHalfway = new Vector3(-planetStartPos.X, planetStartPos.Y, planetStartPos.Z);
            double expectedRadius = planet.Kinematics.Transform.Pos.Magnitude;
            double expectedEnergy = planet.Energy;
            double maxEnergyDifference = 0;
            double maxRadiusDifference = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < halfNumSteps; i++)
            {
                sunEarthUni.Update(timeStep);
                maxRadiusDifference = Math.Max(maxRadiusDifference, Math.Abs(expectedRadius - planet.Kinematics.Transform.Pos.Magnitude));
                maxEnergyDifference = Math.Max(maxEnergyDifference, Math.Abs(expectedEnergy - planet.Energy));
            }
            stopwatch.Stop();
            double halfwayDiff = (expectedHalfway - planet.Kinematics.Transform.Pos).Magnitude;

            stopwatch.Start();
            for (int i = 0; i < halfNumSteps; i++)
            {
                sunEarthUni.Update(timeStep);
                maxRadiusDifference = Math.Max(maxRadiusDifference, Math.Abs(expectedRadius - planet.Kinematics.Transform.Pos.Magnitude));
                maxEnergyDifference = Math.Max(maxEnergyDifference, Math.Abs(expectedEnergy - planet.Energy));
            }
            stopwatch.Stop();
            double endTime = stopwatch.Elapsed.TotalSeconds;
            double endDiff = (planetStartPos - planet.Kinematics.Transform.Pos).Magnitude;

            return new SimResults(2 * halfNumSteps, endTime, halfwayDiff / expectedRadius, endDiff / expectedRadius, maxEnergyDifference / expectedEnergy, maxRadiusDifference / expectedRadius);
        }
    }
}
