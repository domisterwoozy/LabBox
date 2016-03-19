using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D;
using Physics3D.Bodies;

namespace Physics3D.Collisions
{
    public class NoContacts : IContactFinder
    {
        public static readonly NoContacts Instance = new NoContacts();

        private NoContacts() { }

        public IEnumerable<Contact> FindContacts(IBody mainBody, IEnumerable<IBody> otherBodies) => Enumerable.Empty<Contact>();
    }

    public class NoCollisions : IContactResolver
    {
        public static readonly NoCollisions Instance = new NoCollisions();


        public IImpulseEngine Engine => NoImpulse.Instance;

        private NoCollisions() { }

        public bool ResolveContacts(IEnumerable<Contact> contacts) => true;
    }

    public class NoImpulse : IImpulseEngine
    {
        public static readonly NoImpulse Instance = new NoImpulse();

        private NoImpulse() { }

        public double Epsilon { get; set; } = 0.0;

        public Vector3 Collide(Contact c) => Vector3.Zero;
    }
}
