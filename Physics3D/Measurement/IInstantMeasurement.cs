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
    /// <summary>
    /// A measurement that occurs at an instant of time.
    /// TakeMeasurement should be a pure function.
    /// </summary>
    public interface IInstantMeasurement<T>
    {
        string Name { get; }
        T TakeMeasurement(IUniverse uni);
    }

    
}
