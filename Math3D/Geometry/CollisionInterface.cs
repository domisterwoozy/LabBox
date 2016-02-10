namespace Math3D.Geometry
{
    public struct CollisionInterface
    {
        public Vector3 Point { get; }
        public Vector3 Normal { get; }

        public CollisionInterface(Vector3 point, Vector3 normal)
        {
            Point = point;
            Normal = normal;
        }
    }
}
