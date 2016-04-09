using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public abstract class Optional<T>
    {
        public static readonly Optional<T> Nothing = new None();       

        private Optional() { }

        public abstract R Match<R>(Func<T, R> someFunc, Func<None, R> noneFunc);

        public sealed class Some : Optional<T>
        {
            public T Item { get; }
            public Some(T item) { Item = item; }
            public override R Match<R>(Func<T, R> f, Func<None, R> g) => f(Item);
        }

        public sealed class None : Optional<T>
        {
            internal None() { }
            public override R Match<R>(Func<T, R> f, Func<None, R> g) => g(this);
        }
    }

    public static class OptionalExtensions
    {
        public static Optional<T> ToOptional<T>(this T obj) => new Optional<T>.Some(obj);
    }

}
