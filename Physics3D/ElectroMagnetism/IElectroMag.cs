using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.ElectroMagnetism
{
    /// <summary>
    /// Encapsualtes the electromagnetic properties of a body. 
    /// Todo: add more multipole expansions
    /// </summary>
    public interface IElectroMag
    {
        double Charge { get; }        
        Vector3 ElectricDipoleMoment { get; }
        Vector3 MagneticDipoleMoment { get; }
    }
}
