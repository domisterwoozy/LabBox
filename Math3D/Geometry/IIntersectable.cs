using System.Collections.Generic;

namespace Math3D.Geometry
{
    public interface IIntersectable<T> 
    {
        /// <summary>
        /// Determines if and where a specified object intersects this object.
        /// </summary>
        IEnumerable<Intersection> FindIntersections(T other);
    }
}
