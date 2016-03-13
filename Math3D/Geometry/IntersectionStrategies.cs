using Math3D;
using Math3D.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Math3D.Geometry
{
    public class SphereSphereStrategy : Strategy<IEdgeIntersector, Transformation, IEnumerable<Intersection>>
    {
        public SphereSphereStrategy() : base(typeof(SphereIntersectorVolume), typeof(SphereIntersectorVolume)) { }

        protected override IEnumerable<Intersection> EnactStrategyInternal(IEdgeIntersector firstShape, IEdgeIntersector secondShape, Transformation firstToSecond)
        {
            var sphereOne = (SphereIntersectorVolume)firstShape;
            var sphereTwo = (SphereIntersectorVolume)secondShape;

            var firstPos = firstToSecond.TransformA.Pos;
            var secondPos = firstToSecond.TransformB.Pos;

            Vector3 firstPosToSecond = secondPos - firstPos;
            double totalRadius = sphereOne.Radius + sphereTwo.Radius;
            if (firstPosToSecond.Magnitude > totalRadius) yield break;
            yield return new Intersection(firstPos + firstPosToSecond * (sphereOne.Radius / totalRadius), firstPosToSecond.UnitDirection);
        }
    }

    public class SphereCompositeStrategy : Strategy<IEdgeIntersector, Transformation, IEnumerable<Intersection>>
    {
        public SphereCompositeStrategy() : base(typeof(SphereIntersectorVolume), typeof(CompositeIntersector)) { }

        protected override IEnumerable<Intersection> EnactStrategyInternal(IEdgeIntersector firstShape, IEdgeIntersector secondShape, Transformation firstToSecond)
        {
            var sphere = firstShape as SphereIntersectorVolume ?? secondShape as SphereIntersectorVolume;
            var triangles = firstShape as CompositeIntersector ?? secondShape as CompositeIntersector;
            if (sphere == null || triangles == null) throw new ArgumentException("Invalid shape types for this strategy");

            Vector3 spherePos = sphere == firstShape ? firstToSecond.TransformA.Pos : firstToSecond.TransformB.Pos;
            Vector3 trianglesPos = triangles == firstShape ? firstToSecond.TransformA.Pos : firstToSecond.TransformB.Pos;
            

            foreach(TriangleIntersector tri in triangles.Primitives)
            {
                Vector3 trianglePos = trianglesPos + (1.0 / 3.0) * (tri.A + tri.B + tri.C);
                Vector3 sphereToTriangle = trianglePos - spherePos;

                if (sphereToTriangle * tri.N > 0) continue; // triangle is facing away from sphere -> no contact
                // find d in the plane equation (http://mathworld.wolfram.com/Plane.html)
                double d = -trianglePos * tri.N;
                // http://mathworld.wolfram.com/Point-PlaneDistance.html
                double dist = spherePos * tri.N + d;

                if (dist > sphere.Radius) continue;

                Vector3 intersectionPt = spherePos + dist * -tri.N;

                // check if itnersectionPt is b/w the 3 vertices
                bool b1 = Sign(intersectionPt, spherePos, trianglesPos + tri.A, trianglesPos + tri.B) < 0.0;
                bool b2 = Sign(intersectionPt, spherePos, trianglesPos + tri.B, trianglesPos + tri.C) < 0.0;
                bool b3 = Sign(intersectionPt, spherePos, trianglesPos + tri.C, trianglesPos + tri.A) < 0.0;
                if (b1 != b2) continue;
                if (b2 != b3) continue;

                yield return new Intersection(intersectionPt, triangles == firstShape ? tri.N : -tri.N);
            }
        }

        // should refac this into a math library
        // the sign of this represents which side x is on of the plane made by p1 p2 and p3
        private static double Sign(Vector3 x, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var data = new double[4, 4];
            // first col
            data[0, 0] = p1.X;
            data[1, 0] = p1.Y;
            data[2, 0] = p1.Z;
            data[3, 0] = 1;
            // second col
            data[0, 1] = p2.X;
            data[1, 1] = p2.Y;
            data[2, 1] = p2.Z;
            data[3, 1] = 1;
            // third col
            data[0, 2] = p3.X;
            data[1, 2] = p3.Y;
            data[2, 2] = p3.Z;
            data[3, 2] = 1;
            // fourth col
            data[0, 3] = x.X;
            data[1, 3] = x.Y;
            data[2, 3] = x.Z;
            data[3, 3] = 1;
            return determinant(data);
        }

        private static double determinant(double[,] m)
        {
            return
               m[0, 3] * m[1, 2] * m[2, 1] * m[3, 0] - m[0, 2] * m[1, 3] * m[2, 1] * m[3, 0] -
               m[0, 3] * m[1, 1] * m[2, 2] * m[3, 0] + m[0, 1] * m[1, 3] * m[2, 2] * m[3, 0] +
               m[0, 2] * m[1, 1] * m[2, 3] * m[3, 0] - m[0, 1] * m[1, 2] * m[2, 3] * m[3, 0] -
               m[0, 3] * m[1, 2] * m[2, 0] * m[3, 1] + m[0, 2] * m[1, 3] * m[2, 0] * m[3, 1] +
               m[0, 3] * m[1, 0] * m[2, 2] * m[3, 1] - m[0, 0] * m[1, 3] * m[2, 2] * m[3, 1] -
               m[0, 2] * m[1, 0] * m[2, 3] * m[3, 1] + m[0, 0] * m[1, 2] * m[2, 3] * m[3, 1] +
               m[0, 3] * m[1, 1] * m[2, 0] * m[3, 2] - m[0, 1] * m[1, 3] * m[2, 0] * m[3, 2] -
               m[0, 3] * m[1, 0] * m[2, 1] * m[3, 2] + m[0, 0] * m[1, 3] * m[2, 1] * m[3, 2] +
               m[0, 1] * m[1, 0] * m[2, 3] * m[3, 2] - m[0, 0] * m[1, 1] * m[2, 3] * m[3, 2] -
               m[0, 2] * m[1, 1] * m[2, 0] * m[3, 3] + m[0, 1] * m[1, 2] * m[2, 0] * m[3, 3] +
               m[0, 2] * m[1, 0] * m[2, 1] * m[3, 3] - m[0, 0] * m[1, 2] * m[2, 1] * m[3, 3] -
               m[0, 1] * m[1, 0] * m[2, 2] * m[3, 3] + m[0, 0] * m[1, 1] * m[2, 2] * m[3, 3];
        }

    }
}
