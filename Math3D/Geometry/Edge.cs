namespace Math3D.Geometry
{
    public struct Edge
    {
        public Vector3 A { get; }
        public Vector3 B { get; }

        public Edge(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
        }

        public Edge TransformCoords(Transform localTransform, Transform targetTransform) => targetTransform.ToTransformSpace(localTransform.ToWorldSpace(this));
    }
}
