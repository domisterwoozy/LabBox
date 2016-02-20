using Math3D;
using Math3D.Geometry;
using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    public static class InstantMeasurements
    {
        /// <summary>
        /// Performs an instantaneous measurement on the specified body.
        /// </summary>
        public static T Measure<T>(this IBody b, IInstantBodyMeasurement<T> measurement) => measurement.TakeMeasurement(b);

        /// <summary>
        /// Performs an instantaneous measurement on the specified universe.
        /// </summary>
        public static T Measure<T>(this IUniverse uni, IInstantMeasurement<T> measurement) => measurement.TakeMeasurement(uni);

        #region Simple instant body measurements
        public static BasicBodyMeasurement<double> MassMeasurement => new BasicBodyMeasurement<double>("Mass", b => b.Dynamics.Mass);
        public static BasicBodyMeasurement<double> KineticEnergyMeasurement => new BasicBodyMeasurement<double>("Energy", b => b.Dynamics.KineticEnergy);
        public static BasicBodyMeasurement<Vector3> PositionMeasurement => new BasicBodyMeasurement<Vector3>("Position", b => b.Dynamics.Transform.Pos);
        public static BasicBodyMeasurement<Transform> TransformMeasurement => new BasicBodyMeasurement<Transform>("Transform", b => b.Dynamics.Transform);
        #endregion

        #region Useful  body Measurement shortcuts
        public static double Mass(this IBody b) => b.Measure(MassMeasurement);
        public static double Energy(this IBody b) => b.Measure(KineticEnergyMeasurement);
        public static Vector3 Position(this IBody b) => b.Measure(PositionMeasurement);
        public static Transform Transform(this IBody b) => b.Measure(TransformMeasurement);
        #endregion

        #region Simple instant universe measurements
        public static BodyCountMeasurement BodyCount(IVolume vol) => new BodyCountMeasurement(vol);
        public static InstantVolumeMeasurement<double> VolumeSum(IVolume vol, IInstantBodyMeasurement<double> bodyMeas)
        {
            return new InstantVolumeMeasurement<double>($"Total {bodyMeas.Name}", vol, bodyMeas, (sum, b) => sum += bodyMeas.TakeMeasurement(b));
        }
        public static InstantVolumeMeasurement<double> VolumeAvg(IVolume vol, IInstantBodyMeasurement<double> bodyMeas)
        {
            return new InstantVolumeMeasurement<double>($"Average {bodyMeas.Name}", vol, bodyMeas, (sum, b) => sum += bodyMeas.TakeMeasurement(b), sum => sum / vol.TotalVolume);
        }
        public static InstantVolumeMeasurement<double> TotalMass(IVolume vol) => VolumeSum(vol, MassMeasurement);
        public static InstantVolumeMeasurement<double> MassDensity(IVolume vol) => VolumeAvg(vol, MassMeasurement);
        public static InstantVolumeMeasurement<double> TotalKineticEnergy(IVolume vol) => VolumeSum(vol, KineticEnergyMeasurement);
        public static InstantVolumeMeasurement<double> AvgKineticEnergy(IVolume vol) => VolumeAvg(vol, KineticEnergyMeasurement);
        #endregion
    }
}
