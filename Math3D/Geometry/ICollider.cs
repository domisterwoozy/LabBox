using System.Collections.Generic;

namespace Math3D.Geometry
{
    /// <summary>
    /// The most basic geometric object that can be tested for intersections against edges.
    /// Can be a 2 or 3 dimensional object.
    /// </summary>
    public interface ICollider : IIntersectable<Edge>
    {
        /// <summary>
        /// The collection of edges that make up this primitive.
        /// </summary>
        IEnumerable<Edge> Edges { get; }

        /// <summary>
        /// If this primitive is a simple subset of a plane a normal direction can be useful when calculating intersections.
        /// This property is optional and not well defined for some types of primitives (ex: sphere).
        /// If it is not well defined it should return null.
        /// </summary>
        Vector3? Normal { get; }        
    }
}
