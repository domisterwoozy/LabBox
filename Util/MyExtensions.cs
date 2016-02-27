using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class MyExtensions
    {
        public static Func<D, R> Memoize<D, R>(this Func<D, R> f)
        {
            var cache = new ConcurrentDictionary<D, R>();
            return d => cache.GetOrAdd(d, f);
        }

        public static void Add<T>(this ICollection<T> collection, params T[] toAdd)
        {
            foreach (T item in toAdd) collection.Add(item);
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> enumerable, params T[] items) => Enumerable.Union(enumerable, items);

        public static IEnumerable<T> Union<T>(this T item, IEnumerable<T> enumerable) => enumerable.Union(item);
    }
}
