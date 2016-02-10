using System;

namespace Math3D
{
    public struct Matrix3
    {
        /// <summary>
        /// The 3x3 identity matrix.
        /// </summary>
        public readonly static Matrix3 Identity = DiagMatrix(1.0, 1.0, 1.0);
        /// <summary>
        /// The 3x3 null matrix. All elements are zero.
        /// </summary>
	    public readonly static Matrix3 Zero = new Matrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);

        // the components
        // row 1
        public double XX { get; }
        public double XY { get; }
        public double XZ { get; }
        // row 2
        public double YX { get; }
        public double YY { get; }
        public double YZ { get; }
        // row 3
        public double ZX { get; }
        public double ZY { get; }
        public double ZZ { get; }

        /// <summary>
        /// Whether or not this matrix is orthoganol. An orthoganol matrix
        /// inverse is equal to its transpose. It also has a determinant of one.
        /// It also does not change the magnitude of a vector when multiplied by
        /// a vector.
        /// </summary>
        public bool IsOrthoganol
        {
            get
            {
                var det = Determinant;
                return Math.Abs(det - 1.0) <= MathConstants.DoubleEqualityTolerance || Math.Abs(det + 1.0) < MathConstants.DoubleEqualityTolerance;
            }
        }

        public Vector3[] RowVectors => new[] { new Vector3(XX, XY, XZ), new Vector3(YX, YY, YZ), new Vector3(ZX, ZY, ZZ) };
        public Vector3[] ColumnVectors => new[] { new Vector3(XX, YX, ZX), new Vector3(XY, YY, ZY), new Vector3(XZ, YZ, ZZ) };

        public double Determinant => (XX * YY * ZZ + XY * YZ * ZX + XZ * YX * ZY) - (XZ * YY * ZX + XY * YX * ZZ + XX * YZ * ZY); // should we cache this? need to test

        public Matrix3(double xx, double xy, double xz, double yx, double yy, double yz, double zx, double zy, double zz)
        {
            XX = xx;
            XY = xy;
            XZ = xz;
            YX = yx;
            YY = yy;
            YZ = yz;
            ZX = zx;
            ZY = zy;
            ZZ = zz;
        }

        public static Matrix3 DiagMatrix(double xx, double yy, double zz) => new Matrix3(xx, 0, 0, 0, yy, 0, 0, 0, zz);     
        
        public static Matrix3 FromRowVectors(Vector3 one, Vector3 two, Vector3 three) => new Matrix3(one.X, one.Y, one.Z, two.X, two.Y, two.Z, three.X, three.Y, three.Z);        

        public static Matrix3 FromColumnVectors(Vector3 one, Vector3 two, Vector3 three) =>new Matrix3(one.X, two.X, three.X, one.Y, two.Y, three.Y, one.Z, two.Z, three.Z);        

        public Matrix3 InverseMatrix()
        {
            double factor = Determinant;
            if (factor == 0)
            {
                throw new InvalidOperationException("Singular matrix cannot be inverted");
            }
            return new Matrix3(
            (YY * ZZ - YZ * ZY) / factor,
            (XZ * ZY - XY * ZZ) / factor,
            (XY * YZ - XZ * YY) / factor,
            (YZ * ZX - YX * ZZ) / factor,
            (XX * ZZ - XZ * ZX) / factor,
            (XZ * YX - XX * YZ) / factor,
            (YX * ZY - YY * ZX) / factor,
            (XY * ZX - XX * ZY) / factor,
            (XX * YY - XY * YX) / factor);            
        }

        public Matrix3 TransposeMatrix() => new Matrix3(XX, YX, ZX, XY, YY, ZY, XZ, YZ, ZZ);

        public double Magnitude(Vector3 direction)
        {
            direction = direction.UnitDirection;
            return (this * direction) * direction;
        }

        public Matrix3 Add(Matrix3 m)
        {
            return new Matrix3(XX + m.XX, XY + m.XY, XZ + m.XZ,
                YX + m.YX, YY + m.YY, YZ + m.YZ,
                ZX + m.ZX, ZY + m.ZY, ZZ + m.ZZ);
        }

        public Matrix3 Subtract(Matrix3 rhs)
        {
            return new Matrix3(XX - rhs.XX, XY - rhs.XY, XZ - rhs.XZ,
                YX - rhs.YX, YY - rhs.YY, YZ - rhs.YZ,
                ZX - rhs.ZX, ZY - rhs.ZY, ZZ - rhs.ZZ);
        }

        public Matrix3 MultScaler(double scaler)
        {
            return new Matrix3(XX * scaler, XY * scaler, XZ * scaler,
                    YX * scaler, YY * scaler, YZ * scaler,
                    ZX * scaler, ZY * scaler, ZZ * scaler);
        }

        public Matrix3 MultMatrix(Matrix3 rhs)
        {
            return new Matrix3(
            // row 1
            XX * rhs.XX + XY * rhs.YX + XZ * rhs.ZX,
            XX * rhs.XY + XY * rhs.YY + XZ * rhs.ZY,
            XX * rhs.XZ + XY * rhs.YZ + XZ * rhs.ZZ,
            // row 2
            YX * rhs.XX + YY * rhs.YX + YZ * rhs.ZX,
            YX * rhs.XY + YY * rhs.YY + YZ * rhs.ZY,
            YX * rhs.XZ + YY * rhs.YZ + YZ * rhs.ZZ,
            // row 3
            ZX * rhs.XX + ZY * rhs.YX + ZZ * rhs.ZX,
            ZX * rhs.XY + ZY * rhs.YY + ZZ * rhs.ZY,
            ZX * rhs.XZ + ZY * rhs.YZ + ZZ * rhs.ZZ);
        }


        public Matrix3 Rotate(Matrix3 rotationMatrix)
        {
            if (!rotationMatrix.IsOrthoganol) throw new ArgumentException("rotationMatrix must be orthoganol");
            return rotationMatrix * this * rotationMatrix.TransposeMatrix();
        }

        public Matrix3 Rotate(Quaternion rotQuaternion) => Rotate(rotQuaternion.ToMatrix());


        #region Operators
        /// <summary>
        /// Matrix addition.
        /// </summary>
        public static Matrix3 operator +(Matrix3 a, Matrix3 b) => a.Add(b);

        /// <summary>
        /// Vector subtraction. Left side minus the right side.
        /// </summary>
        public static Matrix3 operator -(Matrix3 a, Matrix3 b) => a.Subtract(b);

        /// <summary>
        /// Matrix multiplication. Order matters.
        /// </summary>
        public static Matrix3 operator *(Matrix3 a, Matrix3 b) => a.MultMatrix(b);

        /// <summary>
        /// Scalar matrix muliplication.
        /// </summary>
        public static Matrix3 operator *(double a, Matrix3 b) => b.MultScaler(a);

        public static bool operator ==(Matrix3 a, Matrix3 b) => a.Equals(b);

        public static bool operator !=(Matrix3 a, Matrix3 b) => !(a == b);
        #endregion

        #region Matrix Equality
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is Matrix3)
            {
                return Equals((Matrix3)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Matrix3 m)
        {
            if (XX != m.XX) return false;
            if (XY != m.XY) return false;
            if (XZ != m.XZ) return false;

            if (YX != m.YX) return false;
            if (YY != m.YY) return false;
            if (YZ != m.YZ) return false;

            if (ZX != m.ZX) return false;
            if (ZY != m.ZY) return false;
            if (ZZ != m.ZZ) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + XX.GetHashCode();
            result = 31 * result + XY.GetHashCode();
            result = 31 * result + XZ.GetHashCode();

            result = 31 * result + YX.GetHashCode();
            result = 31 * result + YY.GetHashCode();
            result = 31 * result + YZ.GetHashCode();

            result = 31 * result + ZX.GetHashCode();
            result = 31 * result + ZY.GetHashCode();
            result = 31 * result + ZZ.GetHashCode();
            return result;
        }
        #endregion

        public override string ToString()
        {
            return "Row 1: " + XX + ", " + XY + ", " + XZ + " \n" +
                    "Row 2: " + YX + ", " + YY + ", " + YZ + " \n" +
                    "Row 3: " + ZX + ", " + ZY + ", " + ZZ + " \n";
        }
    }
}
