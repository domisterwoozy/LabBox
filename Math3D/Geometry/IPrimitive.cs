using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public interface IPrimitive
    {
        IEnumerable<Edge> Edges { get; }
        Vector3? Normal { get; }
        IEnumerable<CollisionInterface> IntersectEdge(Edge e);
    }
}
