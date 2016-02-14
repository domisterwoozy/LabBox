using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.ElectroMagnetism
{
    public interface IElectroMag
    {
        double Charge { get; }
        Vector3 MagneticMoment { get; }
    }
}
