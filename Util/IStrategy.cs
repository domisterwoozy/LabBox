using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public interface IStrategy<T, TInput, TOutput>
    {
        UnorderedPair<Type> ValidTypes { get; }
        bool AreObjectsValid(UnorderedPair<T> objects);

        TOutput EnactStrategy(T first, T second, TInput inputData);
    }

    public abstract class Strategy<T, TInput, TOutput> : IStrategy<T, TInput, TOutput>
    {
        public UnorderedPair<Type> ValidTypes { get; }

        protected Strategy(Type t1, Type t2)
        {
            if (!t1.GetInterfaces().Contains(typeof(T))) throw new ArgumentException(nameof(t1));
            if (!t2.GetInterfaces().Contains(typeof(T))) throw new ArgumentException(nameof(t2));
            ValidTypes = new UnorderedPair<Type>(t1, t2);
        }

        public bool AreObjectsValid(UnorderedPair<T> objects) => new UnorderedPair<Type>(objects.Item1.GetType(), objects.Item2.GetType()).Equals(ValidTypes);

        public TOutput EnactStrategy(T first, T second, TInput inputData)
        {
            if (AreObjectsValid(new UnorderedPair<T>(first, second))) return EnactStrategyInternal(first, second, inputData);
            throw new ArgumentException(nameof(first) + " and " + nameof(second) + " are not valid");
        }

        protected abstract TOutput EnactStrategyInternal(T first, T second, TInput inputData);
    }

    public class StrategyContainer<T, TInput, TOutput>
    {
        private readonly Dictionary<UnorderedPair<Type>, IStrategy<T, TInput, TOutput>> strats = new Dictionary<UnorderedPair<Type>, IStrategy<T, TInput, TOutput>>();

        public void AddStrategy(IStrategy<T, TInput, TOutput> strategy)
        {
            strats[strategy.ValidTypes] = strategy;
        }

        public IStrategy<T, TInput, TOutput> GetStrategy(T first, T second) => GetStrategy(new UnorderedPair<T>(first, second));

        public IStrategy<T, TInput, TOutput> GetStrategy(UnorderedPair<T> objects)
        {
            if (!HasStrategy(objects)) return null;
            return strats[new UnorderedPair<Type>(objects.Item1.GetType(), objects.Item2.GetType())];
        }

        public TOutput EnactStrategy(T first, T second, TInput inputData)
        {
            if (!HasStrategy(first, second)) throw new ArgumentException("No valid strategies for these object types.");
            return GetStrategy(first, second).EnactStrategy(first, second, inputData);
        }

        public bool HasStrategy(T first, T second) => HasStrategy(new UnorderedPair<T>(first, second));
        public bool HasStrategy(UnorderedPair<T> objects) => strats.ContainsKey(new UnorderedPair<Type>(objects.Item1.GetType(), objects.Item2.GetType()));
    }
}
