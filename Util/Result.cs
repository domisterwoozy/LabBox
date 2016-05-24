using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
    public static class ResultExtensions
    {
        public static IEnumerable<S> WhereSuccess<S, E>(this IEnumerable<Result<S, E>> enumerableResult)
        {
            return enumerableResult.OfType<Result<S, E>.Okay>().Select(ok => ok.Item); ;
        }

        public static IEnumerable<T> WhereSuccess<T, S, E>(this IEnumerable<T> enumerable, Func<T, Result<S, E>> resultSelector)
        {
            return enumerable.Select(resultSelector).OfType<Result<T, E>.Okay>().Select(ok => ok.Item);
        }

        public static IEnumerable<E> WhereFailed<S, E>(this IEnumerable<Result<S, E>> enumerableResult)
        {
            return enumerableResult.OfType<Result<S, E>.Error>().Select(err => err.ErrorItem); ;
        }

        public static IEnumerable<E> WhereFailed<T, S, E>(this IEnumerable<T> enumerable, Func<T, Result<S, E>> resultSelector)
        {
            return enumerable.Select(resultSelector).OfType<Result<T, E>.Error>().Select(err => err.ErrorItem);
        }

        public static IEnumerable<S> ValidateAll<S, E>(this IEnumerable<Result<S, E>> enumerableResult, Func<E, Exception> toThrow)
            => enumerableResult.Select(r => r.Validate(toThrow));
    }
    public abstract class Result<S, E>
    {
        public abstract K Match<K>(Func<S, K> okFunc, Func<E, K> errorFunc);
        public abstract void Do(Action<S> okAction, Action<E> errorAction);
        public abstract S Validate(Func<E, Exception> toThrow);

        private Result() { }

        public static Result<S, E> Ok(S item) => new Okay(item);
        public static Result<S, E> Err(E errorItem) => new Error(errorItem);

        public static implicit operator Result<S, E>(S item) => Ok(item);        

        public sealed class Okay : Result<S, E>
        {
            public S Item { get; }

            public Okay(S item) { Item = item; }

            public override K Match<K>(Func<S, K> okFunc, Func<E, K> errorFunc) => okFunc(Item);

            public override void Do(Action<S> okAction, Action<E> errorAction) => okAction(Item);

            public override S Validate(Func<E, Exception> toThrow) => Item;
        }

        public sealed class Error : Result<S, E>
        {
            public E ErrorItem { get; }

            public Error(E errorItem) { ErrorItem = errorItem; }

            public override K Match<K>(Func<S, K> okFunc, Func<E, K> errorFunc) => errorFunc(ErrorItem);

            public override void Do(Action<S> okAction, Action<E> errorAction) => errorAction(ErrorItem);

            public override S Validate(Func<E, Exception> toThrow)
            {
                throw toThrow(ErrorItem);
            }
        }
    }
}