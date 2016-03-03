using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class Everything : IVolume
    {
        public static Everything Instance { get; } = new Everything();

        public double TotalVolume => double.PositiveInfinity;
        public Func<Vector3, bool> VolumeFunc => r => true;
        public double SurfaceArea => double.PositiveInfinity;
        public double CrossSectionalArea(Vector3 cutNormal) => double.PositiveInfinity;

        private Everything() { }  
    }
}
