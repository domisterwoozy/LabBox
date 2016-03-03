using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class Triangle : ICollider
    {
        public Vector3 A { get; }
        public Vector3 B { get; }
        public Vector3 C { get; }

        public Vector3 AB => B - A;
        public Vector3 AC => C - A;
        public Vector3 BC => C - B;

        /// <summary>
        /// The direction normal to the plane the triangle is in.
        /// The direction points upward from this plane in accordance with the right hand rule of the 3 triangle points (A, B, C) in that order.
        /// </summary>
        public Vector3 N => (AB ^ AC).UnitDirection;
        /// <summary>
        /// Points outward from the triangle, perpindicular to edge AB but inside the triangle plane.
        /// </summary>
        public Vector3 ABN => (AB ^ N).UnitDirection;
        /// <summary>
        /// Points outward from the triangle, perpindicular to edge AC but inside the triangle plane.
        /// </summary>
        public Vector3 ACN => (N ^ AC).UnitDirection;
        /// <summary>
        /// Points outward from the triangle, perpindicular to edge BC but inside the triangle plane.
        /// </summary>
        public Vector3 BCN => (BC ^ N).UnitDirection;

        public Vector3? Normal => N;
        public IEnumerable<Edge> Edges => new[] { new Edge(A, B), new Edge(A, C), new Edge(B, C) };

        /// <summary>
        /// The points of the triangle are assigned counter clockwise
        /// and the normal points upward.
        /// </summary>
        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            // todo: verify the 3 points are not colinear
            A = a;
            B = b;
            C = c;
        }        

        public IEnumerable<Intersection> FindIntersections(Edge e)
        {
            // points of edge reffered to as p and q
            Vector3 p = e.A;
            Vector3 q = e.B;
            Vector3 pq = q - p;
            double d = pq * N; // abs(d) is dist b/w p and q perp to triangle plane
            if (d == 0) return Enumerable.Empty<Intersection>(); // pq is paralell to the plane

            Vector3 pa = A - p;
            double t = pa * N; // perp dist from triangle plane to p
            if (t * d < 0) return Enumerable.Empty<Intersection>(); // q is further from the plane than p is
            if (Math.Abs(t) > Math.Abs(d)) return Enumerable.Empty<Intersection>(); // pq does not intersect the plane

            // at this point we know the edge intersects the plane that the triangle is in
            // but we don't know if it intersects the actual triangle

            Vector3 r = p + (t / d) * pq; // point of intersection with the plane

            Vector3 ra = A - r;
            double rab = ra * ABN; // dist b/w intersection and segment ab
            if (rab < 0) return Enumerable.Empty<Intersection>(); // outside of segment ab

            Vector3 rb = B - r;
            double rbc = rb * BCN;
            if (rbc < 0) return Enumerable.Empty<Intersection>(); // outside of segment bc

            Vector3 rc = C - r;
            double rac = rc * ACN;
            if (rac < 0) return Enumerable.Empty<Intersection>(); // outside of ac

            return new[] { new Intersection(r, N) };
        }
    }
}
