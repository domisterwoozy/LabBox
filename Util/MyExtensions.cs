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
    }
}
