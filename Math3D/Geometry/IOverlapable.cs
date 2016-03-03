﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public interface IOverlapable
    {
        bool AreOverlapping(IOverlapable other);
    }

    public sealed class NeverOverlap : IOverlapable
    {
        public static readonly NeverOverlap Instance = new NeverOverlap();
        private NeverOverlap() { }

        public bool AreOverlapping(IOverlapable other) => false;
    }

    public sealed class AlwaysOverlap : IOverlapable
    {
        public static readonly AlwaysOverlap Instance = new AlwaysOverlap();
        private AlwaysOverlap() { }

        public bool AreOverlapping(IOverlapable other) => true;
    }
}
