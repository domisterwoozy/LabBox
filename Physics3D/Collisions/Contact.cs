using Math3D.Geometry;
using Physics3D.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Collisions
{
    public class Contact
    {
        public Intersection Intersection { get; }
        public IBody BodyA { get; }
        public IBody BodyB { get; }

        /// <summary>
        /// The intersection normal should be pointing away from body B and towards body A.
        /// The returned impulse should be enacted on body A and the negation enacted on body B.
        /// The location of the impulse is the location of the intersection in world space.
        public Contact(Intersection inter, IBody a, IBody b)
        {
            BodyA = a;
            BodyB = b;
            Intersection = inter;
        }
    }
}
