using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Math3D.Geometry
{
    public class Sphere : IVolume
    {
        public double Radius { get; }

        public double SurfaceArea => 4 * Math.PI * Math.Pow(Radius, 2);

        public double TotalVolume => (4.0 / 3.0) * Math.PI * Math.Pow(Radius, 3);

        public double CrossSectionalArea(Vector3 cutNormal) => 2 * Math.PI * Math.Pow(Radius, 2);

        public Func<Vector3, bool> VolumeFunc => pos => pos.Magnitude <= Radius;

        public Sphere(double radius)
        {
            if (radius <= 0.0) throw new ArgumentException(nameof(radius) + " must be positive");
            Radius = radius;
        }

        
    }    
    
}
