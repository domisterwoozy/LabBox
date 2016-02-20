using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public interface IVolume
    {
        Func<Vector3, bool> VolumeFunc { get; }
        double TotalVolume { get; }
        double SurfaceArea { get; }
    }
}
