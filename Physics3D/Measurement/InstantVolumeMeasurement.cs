using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics3D.Universes;
using Math3D.Geometry;
using Physics3D.Bodies;

namespace Physics3D.Measurement
{
    public class InstantVolumeMeasurement<T> : IInstantMeasurement<T>
    {
        public string Name { get; }
        public IVolume Volume { get; }
        public IInstantBodyMeasurement<T> BodyMeasurement { get; }
        public Func<T, IBody, T> AccumulationFunc { get; }
        public Func<T, T> FinalizeFunc { get; }

        public InstantVolumeMeasurement(string name, IVolume vol, IInstantBodyMeasurement<T> bodyMeasurement, Func<T, IBody, T> accumFunc, Func<T, T> finalizeFunc = null)
        {
            Name = name;
            Volume = vol;
            BodyMeasurement = bodyMeasurement;
            AccumulationFunc = accumFunc;
            FinalizeFunc = finalizeFunc ?? (r => r);
        }

        public T TakeMeasurement(IUniverse uni) => uni.BodiesWithin(Volume).Aggregate(default(T), AccumulationFunc);
    }
}
