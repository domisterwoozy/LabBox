using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public interface IOverlapable
    {
        bool AreOverlapping(IOverlapable other, Vector3 thisPos, Vector3 otherPos);
    }

    public sealed class NeverOverlap : IOverlapable
    {
        public static readonly NeverOverlap Instance = new NeverOverlap();
        private NeverOverlap() { }

        public bool AreOverlapping(IOverlapable other, Vector3 thisPos, Vector3 otherPos) => false;
    }

    public sealed class AlwaysOverlap : IOverlapable
    {
        public static readonly AlwaysOverlap Instance = new AlwaysOverlap();
        private AlwaysOverlap() { }

        public bool AreOverlapping(IOverlapable other, Vector3 thisPos, Vector3 otherPos) => true;
    }

    public sealed class SphereBound : IOverlapable
    {
        public double Radius { get; }

        public SphereBound(double radius)
        {
            Radius = radius;
        }

        public bool AreOverlapping(IOverlapable other, Vector3 thisPos, Vector3 otherPos)
        {
            // naive implementation currently, going to implement some sort of dynamic dispatch later
            var otherSphere = other as SphereBound;
            if (otherSphere == null) return true;
            return (otherPos - thisPos).Magnitude < (Radius + otherSphere.Radius);

        }
    }
}
