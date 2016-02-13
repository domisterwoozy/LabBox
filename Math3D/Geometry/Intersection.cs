namespace Math3D.Geometry
{
    public struct Intersection
    {
        public Vector3 Point { get; }
        public Vector3 Normal { get; }

        public Intersection(Vector3 point, Vector3 normal)
        {
            Point = point;
            Normal = normal;
        }

        public override string ToString() => $"Intersected at {Point} with a normal facing {Normal}";
    }
}
