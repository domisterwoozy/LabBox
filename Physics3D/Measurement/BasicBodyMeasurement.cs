using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics3D.Bodies;

namespace Physics3D.Measurement
{
    public class BasicBodyMeasurement<T> : IInstantBodyMeasurement<T>
    {
        public string Name { get; }
        public Func<IBody, T> MeasurementFunc { get; }

        public BasicBodyMeasurement(string name, Func<IBody, T> measFunc)
        {
            Name = name;
            MeasurementFunc = measFunc;
        }

        public T TakeMeasurement(IBody body) => MeasurementFunc(body);
    }
}
