using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class SphereIntersectorVolume : IEdgeIntersector, IVolume
    {
        private readonly SphereIntersector sphereCollider;
        private readonly Sphere sphere;

        public IEnumerable<Edge> Edges => sphereCollider.Edges;

        public double SurfaceArea => sphere.SurfaceArea;

        public double TotalVolume => sphere.TotalVolume;

        public Func<Vector3, bool> VolumeFunc => sphere.VolumeFunc;

        public double Radius => sphere.Radius;

        public SphereIntersectorVolume(Vector3 pos, double radius, int rank)
        {
            sphereCollider = new SphereIntersector(pos, radius, rank);
            sphere = new Sphere(radius);
        }

        public double CrossSectionalArea(Vector3 cutNormal) => sphere.CrossSectionalArea(cutNormal);

        public IEnumerable<Intersection> FindIntersections(Edge edge) => sphereCollider.FindIntersections(edge);
    }
}
