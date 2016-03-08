using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public class CompositeIntersector : IEdgeIntersector
    {
        public ImmutableArray<IEdgeIntersector> Primitives { get; }

        public IEnumerable<Edge> Edges { get; }

        public CompositeIntersector(IEnumerable<IEdgeIntersector> primitives)
        {
            Primitives = primitives.ToImmutableArray();
            Edges = Primitives.SelectMany(p => p.Edges);
        }

        public IEnumerable<Intersection> FindIntersections(Edge edge) => Primitives.SelectMany(p => p.FindIntersections(edge));
    }

    public static class Intersectors
    {
        public static CompositeIntersector CuboidIntersector(double length, double width, double height)
        {
            var verts = ImmutableArray.CreateBuilder<Vector3>();
            // tops four points in ccw order
            verts.Add(new Vector3(length / 2, width / 2, height / 2)); // 0
            verts.Add(new Vector3(-length / 2, width / 2, height / 2)); // 1
            verts.Add(new Vector3(-length / 2, -width / 2, height / 2)); // 2
            verts.Add(new Vector3(length / 2, -width / 2, height / 2)); // 3

            verts.Add(new Vector3(length / 2, width / 2, -height / 2)); // 4
            verts.Add(new Vector3(-length / 2, width / 2, -height / 2)); // 5
            verts.Add(new Vector3(-length / 2, -width / 2, -height / 2)); // 6
            verts.Add(new Vector3(length / 2, -width / 2, -height / 2)); // 7

            var prims = ImmutableArray.CreateBuilder<TriangleIntersector>();
            // top
            prims.Add(new TriangleIntersector(verts[0], verts[1], verts[2]));
            prims.Add(new TriangleIntersector(verts[0], verts[2], verts[3]));
            // bottom
            prims.Add(new TriangleIntersector(verts[4], verts[6], verts[5]));
            prims.Add(new TriangleIntersector(verts[4], verts[7], verts[6]));
            //front
            prims.Add(new TriangleIntersector(verts[2], verts[7], verts[3]));
            prims.Add(new TriangleIntersector(verts[2], verts[6], verts[7]));
            // back
            prims.Add(new TriangleIntersector(verts[0], verts[5], verts[1]));
            prims.Add(new TriangleIntersector(verts[0], verts[4], verts[5]));
            // right
            prims.Add(new TriangleIntersector(verts[0], verts[7], verts[4]));
            prims.Add(new TriangleIntersector(verts[0], verts[3], verts[7]));
            // left
            prims.Add(new TriangleIntersector(verts[1], verts[6], verts[2]));
            prims.Add(new TriangleIntersector(verts[1], verts[5], verts[6]));

            return new CompositeIntersector(prims);
        }
    }
}
