using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Collisions
{
    public class LoopingContactResolver : IContactResolver
    {
        public IImpulseEngine Engine { get; }
        public int MaxLoops { get; set; } = 100;

        public LoopingContactResolver(IImpulseEngine engine)
        {
            Engine = engine;
        }

        public bool ResolveContacts(IEnumerable<Contact> contacts)
        {
            Contact[] contactArr = contacts.ToArray();
            if (contactArr.Length == 0) return true;

            int loopCount = 0;
            bool stillResolving = true;
            while (stillResolving)
            {
                loopCount++;
                if (loopCount > MaxLoops) return false;
                stillResolving = false;
                foreach (Contact c in contactArr)
                {
                    Vector3 impulse = Engine.Collide(c);
                    if (impulse != Vector3.Zero)
                    {
                        c.BodyA.Dynamics.EnactImpulse(impulse, c.Intersection.Point - c.BodyA.Dynamics.Transform.Pos);
                        c.BodyB.Dynamics.EnactImpulse(-impulse, c.Intersection.Point - c.BodyB.Dynamics.Transform.Pos);
                        stillResolving = true;
                    }
                }
            }
            return true;
        }
    }
}
