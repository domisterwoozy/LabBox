using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Collisions
{
    public interface IContactResolver
    {
        IImpulseEngine Engine { get; }
        bool ResolveContacts(IEnumerable<Contact> contacts);
    }

    
}
