using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class Point : IPrimitiveVolume
    {
        public static readonly Point Instance = new Point();

        public IEnumerable<Edge> OuterEdges => Enumerable.Empty<Edge>();

        public IEnumerable<IPrimitive> Primitives => Enumerable.Empty<IPrimitive>();

        public double SurfaceArea => 0;

        public double TotalVolume => 0;

        public Func<Vector3, bool> VolumeFunc => v => false;

        public double CrossSectionalArea(Vector3 cutPos, Vector3 cutNormal) => 0;

        public IEnumerable<Intersection> IntersectEdge(IPrimitiveVolume other) => Enumerable.Empty<Intersection>();

        private Point() { }
    }
}
