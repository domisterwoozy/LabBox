using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class Point : IEdgeIntersector, IVolume
    {
        public static readonly Point Instance = new Point();

        public IEnumerable<Edge> Edges => Enumerable.Empty<Edge>();

        public IEnumerable<IIntersectable<Edge>> Primitives => Enumerable.Empty<IIntersectable<Edge>>();

        public double SurfaceArea => 0;

        public double TotalVolume => 0;

        public Func<Vector3, bool> VolumeFunc => v => false;

        public double CrossSectionalArea(Vector3 cutNormal) => 0;

        public IEnumerable<Intersection> FindIntersections(Edge edge) => Enumerable.Empty<Intersection>();

        private Point() { }
    }
}
