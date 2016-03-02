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

        public Contact(Intersection inter, IBody a, IBody b)
        {
            BodyA = a;
            BodyB = b;
            Intersection = inter;
        }
    }
}
