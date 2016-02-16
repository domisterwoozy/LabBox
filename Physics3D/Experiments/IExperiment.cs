using Physics3D.Measurement;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Experiments
{
    public interface IExperiment
    {
        IUniverse Universe { get; }
        IEnumerable<IMeasurement> Measurements { get; }
    }
}
