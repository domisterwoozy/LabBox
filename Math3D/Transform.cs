using System;
using System.Diagnostics;

namespace Math3D
{
    /// <summary>
    /// Represents a position and orientation in 3D space. Can be thought of as a cartesian coordinate system.
    /// Todo: performance analysis to see if we should make this an immutable class.
    /// </summary>
    public struct Transform
    {
        public static readonly Transform Zero = new Transform(Vector3.Zero, Matrix3.Identity);

        /// <summary>
        /// The world position of the center of mass of this body.
        /// </summary>
        public Vector3 Pos { get;  }        

        /// <summary>
        /// The orientation of this body in world coordinates.
        /// </summary>
        public Quaternion Q { get; }

        /// <summary>
        /// The orientation of the body in world coordinates.
        /// </summary>
        public Matrix3 R => Q.ToMatrix();

        /// <summary>
        /// The 'i' basis vector of this transform's local coordinates in world coordinates.
        /// </summary>
        public Vector3 I => ToWorldSpace(Vector3.I);

        /// <summary>
        /// The 'j' basis vector of this transform's local coordinates in world coordinates.
        /// </summary>
        public Vector3 J => ToWorldSpace(Vector3.J);

        /// <summary>
        /// The 'k' basis vector of this transform's local coordinates in world coordinates.
        /// </summary>
        public Vector3 K => ToWorldSpace(Vector3.K);

        public Transform(Vector3 pos, Quaternion q)
        {
            if (!q.IsUnit) throw new ArgumentException(nameof(q) + "must be a unit quaternion");
            Pos = pos;
            Q = q;
        }

        public Transform(Vector3 pos, Matrix3 r)
        {
            Pos = pos;
            Q = Quaternion.FromRotMatrix(r);
        }   
        
        /// <summary>
        /// Gets the unit vector directions in world space of this local coordinate spaces basis vectors.
        /// </summary>
        public Vector3 GetBasis(Axis axes)
        {
            switch (axes)
            {
                case Axis.X:
                    return I;
                case Axis.Y:
                    return J;
                case Axis.Z:
                    return K;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axes));
            }
        }     

        /// <summary>
        /// Converts a point in this local transforms coordinates to world coordinates.
        /// </summary>
        public Vector3 ToWorldSpace(Vector3 localPoint) => R * localPoint + Pos;

        /// <summary>
        /// Converts a point in world space to this local transform's coordinate space.
        /// </summary>
        public Vector3 ToTransformSpace(Vector3 worldPoint) => R.TransposeMatrix() * (worldPoint - Pos);

        public Transform Translate(Vector3 v) => new Transform(Pos + v, Q);

        public Transform Rotate(Matrix3 m) => Rotate(Quaternion.FromRotMatrix(m));

        public Transform Rotate(Quaternion q)
        {
            Debug.Assert(q.IsUnit);
            return new Transform(Pos, q * Q);
        }

        #region Equality
        /// <summary>
        /// Transform equality.
        /// </summary>
        public static bool operator ==(Transform a, Transform b) => a.Equals(b);

        /// <summary>
        /// Transform inequality.
        /// </summary>
        public static bool operator !=(Transform a, Transform b) => !(a == b);

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + Pos.GetHashCode();
            result = 31 * result + Q.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Transform t;
            if (obj is Transform)
            {
                t = (Transform)obj;
            }
            else
            {
                return false;
            }

            return Equals(t);
        }

        public bool Equals(Transform other)
        {
            if (Pos != other.Pos) return false;
            return Q == other.Q;
        }
        #endregion
    }
}
