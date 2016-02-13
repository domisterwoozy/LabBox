using System.Collections.Generic;

namespace Math3D.Geometry
{
    public interface IVolume : IIntersectable<IVolume>
    {
        IEnumerable<Edge> OuterEdges { get; }
        IEnumerable<IPrimitive> Primitives { get; }
        double CrossSectionalArea(Vector3 cutPos, Vector3 cutNormal);
    }
}
