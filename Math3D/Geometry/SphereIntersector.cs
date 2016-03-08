using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class SphereIntersector : IIntersectable<Edge>
    {
        private readonly ImmutableArray<Edge> edges;

        public Vector3 CenterPos { get; }
        public double Radius { get; }
        public int Rank { get; }

        public IEnumerable<Edge> Edges => edges;

        public Vector3? Normal => null;

        public SphereIntersector(Vector3 pos, double radius, int rank)
        {
            if (radius <= 0.0) throw new ArgumentException(nameof(radius) + " must be positive");
            if (rank <= 0) throw new ArgumentException(nameof(rank) + " must be positive");
            CenterPos = pos;
            Radius = radius;
            Rank = rank;

            edges = CreateEdges(rank).ToImmutableArray();
        }

        private IEnumerable<Edge> CreateEdges(int rank)
        {
            foreach (int i in Enumerable.Range(0, rank))
            {
                foreach (int j in Enumerable.Range(0, rank / 2))
                {
                    double theta = ((double)i / rank) * 2 * Math.PI;
                    double phi = ((double)j / ((float)rank / 2)) * Math.PI;
                    double x = Radius * Math.Cos(theta) * Math.Sin(phi);
                    double y = Radius * Math.Sin(theta) * Math.Sin(phi);
                    double z = Radius * Math.Cos(phi);
                    yield return new Edge(CenterPos, CenterPos + new Vector3(x, y, z));
                }
            }
            yield return new Edge(CenterPos, CenterPos + Radius * Vector3.I);
            yield return new Edge(CenterPos, CenterPos + Radius * Vector3.J);
            yield return new Edge(CenterPos, CenterPos - Radius * Vector3.I);
            yield return new Edge(CenterPos, CenterPos - Radius * Vector3.J);
            yield return new Edge(CenterPos, CenterPos - Radius * Vector3.K);
        }

        public IEnumerable<Intersection> FindIntersections(Edge edge)
        {
            Vector3 ra = edge.A - CenterPos;
            Vector3 rb = edge.B - CenterPos;
            Vector3 ab = rb - ra;

            // segment equation -> x(t) = a + (b - a)t where t goes from 0 to 1
            // intersection point is where the magnitude of (x(t) - center) equals radius
            // therefore |ra + ab*t| = r
            // using foil -> ra^2 + ab^2*t^2 + t^2*ab^2 + 2t*(ra*ab) = r^2
            // solve quadratic formula ab^2*t^2 + (2*(ra*ab))t - (r^2 - ra^2) = 0
            double A = ab.MagSquared;
            double B = 2 * ra * ab;
            double C = ra.MagSquared - Radius * Radius;
            double D = Math.Sqrt(B * B - 4 * A * C);
            // if descrim is imaginary then the edge does not intersect the sphere
            if (double.IsNaN(D)) yield break;

            double t1 = (-B + D) / (2 * A);
            double t2 = (-B - D) / (2 * A);

            if (t1 < 1.0 && t1 > 0.0) // t1 is an intersection
            {
                Vector3 point = CenterPos + ra + ab * t1;
                Vector3 normal = (point - CenterPos).UnitDirection;
                yield return new Intersection(point, normal);
            }
            if (t2 < 1.0 && t2 > 0.0) // t2 is an intersection
            {
                Vector3 point = CenterPos + ra + ab * t2;
                Vector3 normal = (point - CenterPos).UnitDirection;
                yield return new Intersection(point, normal);
            }
        }
    }

}
