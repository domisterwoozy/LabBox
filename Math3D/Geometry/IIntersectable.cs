using System.Collections.Generic;

namespace Math3D.Geometry
{
    public interface IIntersectable<T> 
    {
        /// <summary>
        /// Determines if and where a specified edge intersects this object.
        /// </summary>
        IEnumerable<CollisionInterface> IntersectEdge(T other);
    }
}
