using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public interface IOverlapable
    {
        bool OverlapTest(IOverlapable other);
    }

    public sealed class NeverOverlap : IOverlapable
    {
        public static readonly NeverOverlap Instance = new NeverOverlap();
        private NeverOverlap() { }

        public bool OverlapTest(IOverlapable other) => false;
    }
}
