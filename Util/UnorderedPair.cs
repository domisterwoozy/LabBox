using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{    
    public struct UnorderedPair<T> : IEquatable<UnorderedPair<T>>
    {
        public T Item1 { get; }
        public T Item2 { get; }

        public UnorderedPair(T item1, T item2)
        {
            Item1 = item1;
            Item2 = item2;
        }      

        public bool Equals(UnorderedPair<T> other) => (other.Item1.Equals(Item1) && other.Item2.Equals(Item2)) || (other.Item1.Equals(Item2) && other.Item2.Equals(Item1));

        public override bool Equals(object obj)
        {
            if (obj is UnorderedPair<T>) return Equals((UnorderedPair<T>)obj);
            return false;
        }
                
        public override int GetHashCode() => Item1.GetHashCode() * Item2.GetHashCode(); // is communitative since it is unordered (maybe there is a better way)?

        public override string ToString() => $"({Item1}, {Item2})";        
    }
}
