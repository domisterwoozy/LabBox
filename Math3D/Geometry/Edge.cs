using System;

namespace Math3D.Geometry
{
    public struct Edge : IEquatable<Edge>
    {
        public Vector3 A { get; }
        public Vector3 B { get; }

        public Edge(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
        }

        public bool Equals(Edge other) => A == other.A && B == other.B;

        public override bool Equals(object obj)
        {
            var otherEdge = obj as Edge?;
            if (!otherEdge.HasValue) return false;
            return Equals(otherEdge.Value);
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + A.GetHashCode();
            result = 31 * result + B.GetHashCode();
            return result;
        }

        public override string ToString() => $"Edge From {A} to {B}";
    }
}
