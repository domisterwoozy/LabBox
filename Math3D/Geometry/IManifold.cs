namespace Math3D.Geometry
{
    public interface IManifold
    {
        /// <summary>
        /// Maps an arbitrary point in 3D space to the closest point on the surface.
        /// </summary>
        /// <param name="point">Any point in 3d space.</param>
        /// <returns>A point on the manifolds surface.</returns>
        Vector3 MapToManifold(Vector3 point);

        /// <summary>
        /// Returns a vector that originates at a specified point on the surface and points perpindicular to the manifold.
        /// </summary>
        Vector3 PerpVector(Vector3 pointOnSurface);

        /// <summary>
        /// Determines if the specified point is on the surface of this manifold to within an internal tolerance.
        /// </summary>
        bool IsOnManifold(Vector3 point);
    }
}
