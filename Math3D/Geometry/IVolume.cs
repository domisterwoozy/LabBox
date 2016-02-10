using System.Collections.Generic;

namespace Math3D.Geometry
{
    public interface IVolume : IIntersectable<IVolume>
    {
        IEnumerable<Edge> Edges { get; }
        IEnumerable<IPrimitive> Primitives { get; }
    }
}
