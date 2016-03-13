using System;

namespace Math3D.Geometry
{
    public struct Intersection : IEquatable<Intersection>
    {
        public Vector3 Point { get; }
        public Vector3 Normal { get; }

        public Intersection(Vector3 point, Vector3 normal)
        {
            Point = point;
            Normal = normal;
        }

        public bool Equals(Intersection other) => Point == other.Point && Normal == other.Normal;

        public override bool Equals(object obj)
        {
            var otherInter = obj as Intersection?;
            if (!otherInter.HasValue) return false;
            return Equals(otherInter.Value);
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + Point.GetHashCode();
            result = 31 * result + Normal.GetHashCode();
            return result;
        }

        public override string ToString() => $"Intersected at {Point} with a normal facing {Normal}";
    }
}
