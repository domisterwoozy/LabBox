using NUnit.Framework;

namespace MathTests
{
    public class RandomDoubleAttribute : RandomAttribute
    {
        public RandomDoubleAttribute(int count) : base(-1.0, 1.0, count)
        {
        }
    }    
}
