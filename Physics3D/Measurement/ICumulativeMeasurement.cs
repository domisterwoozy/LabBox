using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    /// <summary>
    /// A measurement on a universe that occurs over an indeterminate amount of time.
    /// </summary>
    public interface ICumulativeMeasurement<T>
    {
        string Name { get; }
        T TotalReading { get; }
        T AddReading(IUniverse uni);
    }

    /// <summary>
    /// A measurement on a body that occurs over an indeterminate amount of time.
    /// </summary>
    public interface ICumulativeBodyMeasurement<T>
    {
        string Name { get; }
        T TotalReading { get; }
        T AddReading(IBody uni);
    }
}
