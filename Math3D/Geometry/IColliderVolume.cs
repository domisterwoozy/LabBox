using System.Collections.Generic;

namespace Math3D.Geometry
{
    public interface IColliderVolume : IIntersectable<IColliderVolume>, IVolume
    {
        IEnumerable<Edge> OuterEdges { get; }
        IEnumerable<ICollider> Primitives { get; }
        double CrossSectionalArea(Vector3 cutPos, Vector3 cutNormal);
    }


}
