using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class Cuboid : IVolume
    {
        public double Length { get; }
        public double Width { get; }
        public double Height { get; }

        public double SurfaceArea => 2 * (Length * Width + Width * Height + Height * Length);

        public double TotalVolume => Length * Width * Height;

        public Func<Vector3, bool> VolumeFunc => pt => ContainsPoint(pt);

        public Cuboid(double length, double width, double height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        public double CrossSectionalArea(Vector3 cutNormal)
        {
            throw new NotImplementedException();
        }

        private bool ContainsPoint(Vector3 pt)
        {
            if (pt.X > Length / 2 || pt.X < Length / 2) return false;
            if (pt.Y > Width / 2 || pt.Y < Width / 2) return false;
            if (pt.Z > Height / 2 || pt.Z < Height / 2) return false;
            return true;
        }
    }
}
