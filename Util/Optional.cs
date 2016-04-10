using System;

namespace Util
{
    public abstract class Optional<T> : IEquatable<Optional<T>>
    {
        public static readonly Optional<T> Nothing = new None();       

        private Optional() { }

        public abstract R Match<R>(Func<T, R> someFunc, Func<R> noneFunc);
        public abstract void Do(Action<T> someAction, Action noneAction);
        public abstract bool Equals(Optional<T> other);

        public override bool Equals(object obj)
        {
            var other = obj as Optional<T>;
            if (other == null) return false;
            return Equals(other);
        }
        public override int GetHashCode() => Match(some => some.GetHashCode(), () => 0);

        public sealed class Some : Optional<T>
        {
            public T Item { get; }
            public Some(T item) { Item = item; }
            public override R Match<R>(Func<T, R> someFunc, Func<R> noneFunc) => someFunc(Item);
            public override void Do(Action<T> someAction, Action noneAction) => someAction(Item);

            public override bool Equals(Optional<T> other)
            {
                return other.Match(
                    some => some.Equals(Item),
                    () => false
                );
            }
        }

        public sealed class None : Optional<T>
        {
            internal None() { }
            public override R Match<R>(Func<T, R> someFunc, Func<R> noneFunc) => noneFunc();
            public override void Do(Action<T> someAction, Action noneAction) => noneAction();

            public override bool Equals(Optional<T> other)
            {
                return other.Match(
                    some => false,
                    () => true
                );
            }
        }
    }

    public static class OptionalExtensions
    {
        public static Optional<T> ToOptional<T>(this T obj) => new Optional<T>.Some(obj);
    }

}
