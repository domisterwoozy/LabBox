using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D
{
    public static class MathExtensions
    {
        public static Vector3 Sum(this IEnumerable<Vector3> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            Vector3 sum = Vector3.Zero;
            checked
            {
                foreach (Vector3 v in source) sum += v;
            }
            return sum;
        }

        public static Quaternion Sum(this IEnumerable<Quaternion> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            Quaternion sum = default(Quaternion);
            checked
            {
                foreach (Quaternion q in source) sum += q;
            }
            return sum;
        }

        public static Vector3 Average(this IEnumerable<Vector3> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            Vector3 sum = default(Vector3);
            long count = 0;
            checked
            {
                foreach(Vector3 v in source)
                {
                    sum += v;
                    count++;
                }
            }
            if (count > 0) return ((double)1 / count) * sum;
            throw new ArgumentException(nameof(source) + " sequence cannot be empty.");
        }

        public static Quaternion Average(this IEnumerable<Quaternion> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            Quaternion sum = default(Quaternion);
            long count = 0;
            checked
            {
                foreach (Quaternion q in source)
                {
                    sum += q;
                    count++;
                }
            }
            if (count > 0) return ((double)1 / count) * sum;
            throw new ArgumentException(nameof(source) + " sequence cannot be empty.");
        }

        public static Vector3 Average<T>(this IEnumerable<T> enumerable, Func<T, Vector3> selector) => Average(enumerable.Select(selector));

        public static Vector3 Sum<T>(this IEnumerable<T> enumerable, Func<T, Vector3> selector) => Sum(enumerable.Select(selector));

        public static Quaternion Average<T>(this IEnumerable<T> enumerable, Func<T, Quaternion> selector) => Average(enumerable.Select(selector));

        public static Quaternion Sum<T>(this IEnumerable<T> enumerable, Func<T, Quaternion> selector) => Sum(enumerable.Select(selector));

        public static float[] ToArray(this Vector3 v) => new[] { (float)v.X, (float)v.Y, (float)v.Z };

        
    }
}
