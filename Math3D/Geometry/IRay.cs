using System;
using System.Collections.Generic;
using Util;

namespace Math3D.Geometry
{
    public struct Hit
    {
        public TransformedObj<IEdgeIntersector> HitObject { get; }
        public Vector3 HitPos { get; }

        internal Hit(TransformedObj<IEdgeIntersector> hitObject, Vector3 hitPos)
        {
            HitObject = hitObject;
            HitPos = hitPos;
        }
    }

    public interface IRay
    {
        Optional<Hit> Cast(IEnumerable<TransformedObj<IEdgeIntersector>> transformedIntersectors, Vector3 origin, Vector3 dir, double rayLength,
            Func<TransformedObj<IEdgeIntersector>, bool> filter = null);
    }
}