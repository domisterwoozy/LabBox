using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D
{
    public static class MathExtensions
    {
        public static Vector3 Sum<T>(this IEnumerable<T> enumerable, Func<T, Vector3> selector)
        {
            throw new NotImplementedException();
        }
    }
}
