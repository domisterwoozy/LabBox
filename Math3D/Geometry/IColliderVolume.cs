using System;
using System.Collections.Generic;

namespace Math3D.Geometry
{
    public interface IColliderVolume : IIntersectable<Edge>, IVolume
    {
        IEnumerable<Edge> OuterEdges { get; }
        IEnumerable<ICollider> Primitives { get; }
    } 
}
