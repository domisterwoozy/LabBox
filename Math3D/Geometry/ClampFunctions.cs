using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Geometry
{
    public static class ClampFunctions
    {
        public static Func<Vector3, bool> OutsideSphere(Vector3 center, double radius) => pos => (pos - center).MagSquared > (radius * radius);

        public static Func<Vector3, bool> OutsideBox(double length, double width, double height, Vector3 center = default(Vector3))
        {
            return OutsideBox(length, width, height, center, Matrix3.Identity);
        }

        public static Func<Vector3, bool> OutsideBox(double length, double width, double height, Vector3 center, Matrix3 orientation)
        {
            if (!orientation.IsOrthoganol) throw new ArgumentException(nameof(orientation) + "must be orthoganol.");
            return pos =>
            {
                Vector3 r = (pos - center).Rotate(orientation.InverseMatrix());
                if (Math.Abs(r.X) > length) return true;
                if (Math.Abs(r.Y) > width) return true;
                if (Math.Abs(r.Z) > height) return true;
                return false;
            };
        }

        public static Func<Vector3, bool> OutsideCylinder(double length, double radius, Vector3 center = default(Vector3))
        {
            return OutsideCylinder(length, radius, center, Vector3.K);
        }

        public static Func<Vector3, bool> OutsideCylinder(double height, double radius, Vector3 center, Vector3 zDir)
        {
            Quaternion rot = zDir.RotationRequired(Vector3.K);
            return pos =>
            {
                Vector3 r = (pos - center).Rotate(rot);
                if (Math.Sqrt(r.X * r.X + r.Y * r.Y) > radius) return true;
                if (Math.Abs(r.Z) > (height / 2)) return true;
                return false;
            };
        }


    }
}
