using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Math3D.Geometry
{
    public static class Ray
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

        public static Optional<Hit> Cast(IEnumerable<TransformedObj<IEdgeIntersector>> transformedIntersectors, Vector3 origin, Vector3 dir, double rayLength,
            Func<TransformedObj<IEdgeIntersector>, bool> filter = null)
        {
            if (transformedIntersectors == null) throw new ArgumentNullException(nameof(transformedIntersectors));
            if (dir == Vector3.Zero) throw new ArgumentException(nameof(dir) + " must not be zero");
            if (filter != null) transformedIntersectors = transformedIntersectors.Where(filter);

            var edge = new Edge(origin, origin + rayLength * dir.UnitDirection);
            foreach (var transformedIntersector in transformedIntersectors.OrderBy(o => (o.ObjTransform.Pos - origin).MagSquared))
            {
                Edge edgeLocal = transformedIntersector.ObjTransform.ToLocalSpace(edge);
                IEnumerable<Intersection> inters = transformedIntersector.Obj.FindIntersections(edge);
                if (!inters.Any()) continue;
                var closest = inters.OrderBy(i => (i.Point - origin).MagSquared).FirstOrDefault();
                return new Hit(transformedIntersector, transformedIntersector.ObjTransform.ToWorldSpace(closest.Point)).ToOptional();
            }
            return Optional<Hit>.Nothing;
        }
    }
}
