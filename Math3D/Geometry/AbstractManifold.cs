namespace Math3D.Geometry
{
    public abstract class AbstractManifold : IManifold
    {
        public abstract Vector3 MapToManifold(Vector3 point);
        public abstract Vector3 PerpVector(Vector3 pointOnSurface);

        public virtual bool IsOnManifold(Vector3 point)
        {
            double dist = (point - MapToManifold(point)).Magnitude; // distance b/w point and closest point on manifold
            return dist <= MathConstants.DoubleEqualityTolerance;
        }

        
    }
}
