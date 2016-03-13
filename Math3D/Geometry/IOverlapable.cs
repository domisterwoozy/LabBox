using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public interface IOverlapable
    {
    }

    public sealed class PointBound : IOverlapable
    {
        public static readonly PointBound Instance = new PointBound();
        private PointBound() { }
    }

    public sealed class InifiniteBound : IOverlapable
    {
        public static readonly InifiniteBound Instance = new InifiniteBound();
        private InifiniteBound() { }
    }

    public sealed class SphereBound : IOverlapable
    {
        public double Radius { get; }

        public SphereBound(double radius)
        {
            Radius = radius;
        }
    }

    public sealed class BoxBound : IOverlapable
    {
        public double Length { get; }
        public double Width { get; }
        public double Height { get; }

        public BoxBound(double length, double width, double height)
        {
            Length = length;
            Width = width;
            Height = height;
        }
    }
}
