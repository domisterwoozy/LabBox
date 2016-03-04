using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class CuboidColliderVolume : IColliderVolume
    {
        private Vector3 pos;
        private double x;
        private double y;
        private double z;

        public CuboidColliderVolume(double x, double y, double z, Vector3 pos)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.pos = pos;
        }

        public IEnumerable<Edge> OuterEdges
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ICollider> Primitives
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double SurfaceArea
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double TotalVolume
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Func<Vector3, bool> VolumeFunc
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double CrossSectionalArea(Vector3 cutNormal)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Intersection> FindIntersections(Edge other)
        {
            throw new NotImplementedException();
        }
    }
}
