using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    /// <summary>
    /// A method that generates a type T based on some internal captured state.
    /// This method is not a pure function and the generated value can change based on the internal state.
    /// </summary>
    public delegate T Generator<out T>();
}
