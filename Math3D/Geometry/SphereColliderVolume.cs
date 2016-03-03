using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class SphereColliderVolume : IColliderVolume
    {
        private readonly SphereCollider sphereCollider;
        private readonly Sphere sphere;

        public IEnumerable<Edge> OuterEdges => sphereCollider.Edges;

        public IEnumerable<ICollider> Primitives { get { yield return sphereCollider; } }

        public double SurfaceArea => sphere.SurfaceArea;

        public double TotalVolume => sphere.TotalVolume;

        public Func<Vector3, bool> VolumeFunc => sphere.VolumeFunc;

        public SphereColliderVolume(Vector3 pos, double radius, int rank)
        {
            sphereCollider = new SphereCollider(pos, radius, rank);
            sphere = new Sphere(radius);
        }

        public double CrossSectionalArea(Vector3 cutNormal) => sphere.CrossSectionalArea(cutNormal);

        public IEnumerable<Intersection> FindIntersections(Edge edge) => sphereCollider.FindIntersections(edge);
    }
}
