using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Collisions
{
    public interface ICollisionEngine
    {
        IContactFinder ContactFinder { get; }
        IContactResolver ContactResolver { get; }
        IImpulseEngine ImpulseEngine { get; }
    }
}
