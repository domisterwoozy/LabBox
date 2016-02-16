using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    public interface IMeasurement
    {
        IReading CurrentReading { get; }
        IReading CumulativeReading { get; }
    }

    public interface IReading
    {
    }
}
