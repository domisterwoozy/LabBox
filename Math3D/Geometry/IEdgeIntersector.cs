using System;
using System.Collections.Generic;

namespace Math3D.Geometry
{
    // <summary>
    // The most basic geometric object that can be tested for intersections against edges.
    // Can be a 2 or 3 dimensional object.
    // </summary>
    public interface IEdgeIntersector : IIntersectable<Edge>
    {
        /// <summary>
        /// The collection of edges that make up this primitive. These should only be 'external' edges that can intersect other objects.
        /// </summary>
        IEnumerable<Edge> Edges { get; }      
    }  
}
