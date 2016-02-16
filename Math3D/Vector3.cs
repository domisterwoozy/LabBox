using System;

namespace Math3D
{
    /// <summary>
    /// An immutable 3 dimensional cartesian vector.
    /// The default value of the struct is equal to Vector3.Zero
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// The zero vector. All elements are zero.
        /// </summary>
        public static readonly Vector3 Zero = default(Vector3);

        /// <summary>
        /// Unit vector in the positive x direction.
        /// </summary>
        public static readonly Vector3 I = new Vector3(1, 0, 0);

        /// <summary>
        /// Unit vector in the positive y direction.
        /// </summary>
        public static readonly Vector3 J = new Vector3(0, 1, 0);

        /// <summary>
        /// Unit vector in the positive z direction.
        /// </summary>
        public static readonly Vector3 K = new Vector3(0, 0, 1);

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public double X { get; }
        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public double Y { get; }
        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public double Z { get; }

        /// <summary>
        /// Creates a 3D cartesian vector from three scalar components.
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        /// <param name="z">Z component</param>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// The additive inverse of this vector.
        /// </summary>
        public Vector3 Inverse => MultScalar(-1.0);

        public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);

        public double MagSquared => X * X + Y * Y + Z * Z;

        public Vector3 UnitDirection
        {
            get
            {
                double mag = Magnitude;
                if (mag == 0) return Zero;
                else
                {
                    return new Vector3(X / mag, Y / mag, Z / mag);
                }
            }
        }

        /// <summary>
        /// Vector addition.
        /// </summary>
        public Vector3 Add(Vector3 other) => new Vector3(X + other.X, Y + other.Y, Z + other.Z);

        /// <summary>
        /// Vector subtraction. This vector minus rhs vector.
        /// </summary>
        public Vector3 Subtract(Vector3 rhs) => new Vector3(X - rhs.X, Y - rhs.Y, Z - rhs.Z);

        /// <summary>
        /// Scalar * Vector multiplication.
        /// </summary>
        public Vector3 MultScalar(double scaler) => new Vector3(X * scaler, Y * scaler, Z * scaler);

        /// <summary>
        /// Vector cross product.
        /// </summary>
        public Vector3 CrossProduct(Vector3 rhs) => new Vector3(Y * rhs.Z - Z * rhs.Y, Z * rhs.X - X * rhs.Z, X * rhs.Y - Y * rhs.X);

        /// <summary>
        /// Vector dot product.
        /// </summary>
        public double DotProduct(Vector3 other) => X * other.X + Y * other.Y + Z * other.Z;

        /// <summary>
        /// Multiplies this vector by a matrix.
        /// Where this vector is on the right and the matrix is on the left.
        /// </summary>
        public Vector3 MultMatrixLeft(Matrix3 lhs)
        {
            return new Vector3(X * lhs.XX + Y * lhs.XY + Z * lhs.XZ,
                           X * lhs.YX + Y * lhs.YY + Z * lhs.YZ,
                           X * lhs.ZX + Y * lhs.ZY + Z * lhs.ZZ);
        }

        /// <summary>
        /// Calculates the rotation required to orient this vector in the direction of another vector.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Quaternion RotationRequired(Vector3 dir)
        {
            double angleDiff = AngleBetween(dir);
            Vector3 rotAxis = this ^ dir; // axis to rotate around to get there
            return Quaternion.UnitQuaternion(angleDiff, rotAxis);
        }

        /// <summary>
        /// Returns a vector that is the projection of this vector onto a plane.
        /// The resultant vector will always be paralell with the plane and
        /// less than 90 degrees away from this vector.
        /// </summary>
        /// <param name="planeNormal">A vector in the direction of the plane normal.</param>
        public Vector3 ProjectToPlane(Vector3 planeNormal)
        {
            planeNormal = planeNormal.UnitDirection;
            return this - (this * planeNormal) * planeNormal;
        }

        /// <summary>
        /// Returns a vector that is the project of this vector onto another vector.
        /// The resultant vector will always have the same direction as this vector.
        /// </summary>
        public Vector3 ProjectToVector(Vector3 otherVector) => ((this * otherVector) / otherVector.MagSquared) * otherVector.UnitDirection;

        /// <summary>
        /// Returns a vector that is this vector rotated by an orthoganol
        /// rotation matrix. The argument matrix must be orthoganol.
        /// </summary>
        public Vector3 Rotate(Matrix3 rotationMatrix)
        {
            if (!rotationMatrix.IsOrthoganol) throw new ArgumentException("rotationMatrix must be orthogonal");
            return rotationMatrix * this;
        }

        public Vector3 Rotate(Quaternion rotQuat) => Rotate(rotQuat.ToMatrix());

        /// <summary>
        /// Calculates the smallest angle between this vector and another vector.
        /// The result is in radians.
        /// </summary>
        public double AngleBetween(Vector3 other)
        {
            double x = (this * other) / (Magnitude * other.Magnitude);
            // fix floating point errors
            if (x > 1.0) x = 1.0;
            else if (x < -1.0) x = -1.0;
            return Math.Acos(x);
        }

        public override string ToString() => "X: " + X + " Y: " + Y + " Z: " + Z;

        #region Operators
        /// <summary>
        /// Vector addition.
        /// </summary>
        public static Vector3 operator +(Vector3 a, Vector3 b) => a.Add(b);

        /// <summary>
        /// Vector subtraction. Left side minus the right side.
        /// </summary>
        public static Vector3 operator -(Vector3 a, Vector3 b) => a.Subtract(b);

        /// <summary>
        /// The negation of a vector.
        /// </summary>
        public static Vector3 operator -(Vector3 v) => -1 * v;

        /// <summary>
        /// Vector cross product.
        /// </summary>
        public static Vector3 operator ^(Vector3 a, Vector3 b) => a.CrossProduct(b);

        /// <summary>
        /// Vector dot product.
        /// </summary>
        public static double operator *(Vector3 a, Vector3 b) => a.DotProduct(b);

        /// <summary>
        /// Scalar * Vector multiplciation.
        /// </summary>
        public static Vector3 operator *(double a, Vector3 b) => b.MultScalar(a);

        /// <summary>
        /// Matrix * Vector multiplication
        /// </summary>
        public static Vector3 operator *(Matrix3 a, Vector3 b) => b.MultMatrixLeft(a);

        /// <summary>
        /// Vector equality.
        /// </summary>
        public static bool operator ==(Vector3 a, Vector3 b) => a.Equals(b);

        /// <summary>
        /// Vector inequality.
        /// </summary>
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);
        #endregion

        #region Vector Equality
        public bool Equals(Vector3 v)
        {
            if (X != v.X) return false;
            if (Y != v.Y) return false;
            if (Z != v.Z) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + X.GetHashCode();
            result = 31 * result + Y.GetHashCode();
            result = 31 * result + Z.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Vector3 v;
            if (obj is Vector3) v = (Vector3)obj;            
            else return false;           
            return Equals(v);
        }
        #endregion
    }
}
