using System;

namespace Math3D
{
    /// <summary>
    /// An immutable structure representing a quaternion.
    /// A unit quaternion (magnitude = s^2 + |v|2 = 1) is a fast and efficient way to represent an orientation in 3D space.
    /// There exists a mapping between a unit quaternion and a orthoganol rotation matrix (the ToMatrix() method)
    /// This structure can represent any quaternion (not just unit). 
    /// </summary>
    public struct Quaternion : IEquatable<Quaternion>
    {
        // the fields
        public double S { get; }
        public Vector3 V { get; }

        private Quaternion(double s, Vector3 v)
        {
            S = s;
            V = v;
        }

        /// <summary>
        /// Creates a unit quaternion that represents an angle of rotation around a specified axis.
        /// </summary>
        /// <param name="angle">the angle in radians</param>
        /// <param name="dir">the direction to orient around</param>
        public static Quaternion UnitQuaternion(double angle, Vector3 dir)
        {
            Vector3 v = Math.Sin(angle / 2) * dir.UnitDirection;
            Quaternion q = new Quaternion(Math.Cos(angle / 2), v);
            return q;
        }

        public static Quaternion FromRotMatrix(Matrix3 rot)
        {
            if (!rot.IsOrthoganol) throw new ArgumentException(nameof(rot) + " must be an orthoganol matrix");
            double s = Math.Sqrt(1 + rot.XX + rot.YY + rot.ZZ) / 2;
            double denom = 4 * s;
            return new Quaternion(s, new Vector3((rot.ZY - rot.YZ) / denom, (rot.XZ - rot.ZX) / denom, (rot.YX - rot.XY) / denom));
        }

        /// <summary>
        /// A (non-unit) quaternion that represents a virtual vector. The S component of this quaternion is zero and the V component
        /// is the vector. Useful for certain calculations.
        /// </summary>
        public static Quaternion VectorQ(Vector3 v) => new Quaternion(0, v);                  

        public double Magnitude => Math.Sqrt(MagSquared);

        public double MagSquared => S * S + V.MagSquared;

        public bool IsUnit => (MagSquared - 1) < MathConstants.DoubleEqualityTolerance;          

        public override string ToString() => "S: " + S + " " + V.ToString();

        public Quaternion MultScalar(double scaler) => new Quaternion(S * scaler, scaler * V);

        public Quaternion Add(Quaternion rhs) => new Quaternion(S + rhs.S, V.Add(rhs.V));

        public Quaternion Subtract(Quaternion rhs) => new Quaternion(S - rhs.S, V.Subtract(rhs.V));

        public Quaternion Inverse() => Conjugate().MultScalar(1 / MagSquared);

        public Quaternion Conjugate() => new Quaternion(S, V.Inverse);

        public Quaternion MultQuat(Quaternion rhs)
        {
            double newS = S * rhs.S - V.DotProduct(rhs.V);
            Vector3 newV = V.CrossProduct(rhs.V).Add(rhs.V.MultScalar(S).Add(V.MultScalar(rhs.S)));
            return new Quaternion(newS, newV);
        }

        public Quaternion Normalized()
        {
            double mag = Magnitude;
            return new Quaternion(S / mag, V.MultScalar(1 / mag));            
        }

        public Matrix3 ToMatrix()
        {
            return new Matrix3(1 - 2 * V.Y * V.Y - 2 * V.Z * V.Z,
                    2 * V.X * V.Y - 2 * S * V.Z,
                    2 * V.X * V.Z + 2 * S * V.Y,
                    2 * V.X * V.Y + 2 * S * V.Z,
                    1 - 2 * V.X * V.X - 2 * V.Z * V.Z,
                    2 * V.Y * V.Z - 2 * S * V.X,
                    2 * V.X * V.Z - 2 * S * V.Y,
                    2 * V.Y * V.Z + 2 * S * V.X,
                    1 - 2 * V.X * V.X - 2 * V.Y * V.Y);
        }

        #region Operators
        /// <summary>
        /// Quaternion addition.
        /// </summary>
        public static Quaternion operator +(Quaternion a, Quaternion b) => a.Add(b);

        /// <summary>
        /// Quaternion subtraction. Left side minus the right side.
        /// </summary>
        public static Quaternion operator -(Quaternion a, Quaternion b) => a.Subtract(b); 

        /// <summary>
        /// Scalar * Quaternion multiplciation.
        /// </summary>
        public static Quaternion operator *(double a, Quaternion b) => b.MultScalar(a);

        /// <summary>
        /// Scalar * Quaternion multiplciation.
        /// </summary>
        public static Quaternion operator *(Quaternion a, Quaternion b) => a.MultQuat(b);

        /// <summary>
        /// Quaternion equality.
        /// </summary>
        public static bool operator ==(Quaternion a, Quaternion b) => a.Equals(b);

        /// <summary>
        /// Quaternion inequality.
        /// </summary>
        public static bool operator !=(Quaternion a, Quaternion b) => !(a == b);
        #endregion

        #region Equality
        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + V.GetHashCode();
            result = 31 * result + S.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Quaternion q;
            if (obj is Quaternion)
            {
                q = (Quaternion)obj;
            }
            else
            {
                return false;
            }

            return Equals(q);
        }

        public bool Equals(Quaternion other)
        {
            if (S != other.S) return false;
            return V == other.V;
        }
        #endregion
    }
}
