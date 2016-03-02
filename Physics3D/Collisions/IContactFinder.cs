using Math3D.Geometry;
using Physics3D.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Collisions
{
    public interface IContactFinder
    {
        IEnumerable<Contact> FindContacts(IBody mainBody, IEnumerable<IBody> otherBodies);
    }

    public class BasicContactFinder : IContactFinder
    {
        public IEnumerable<Contact> FindContacts(IBody mainBody, IEnumerable<IBody> otherBodies)
        {
            foreach(IBody b in otherBodies)
            {
                if (!mainBody.BoundVolume.AreOverlapping(b.BoundVolume)) continue;
                foreach (Intersection a in b.CollisionShape.IntersectEdge(b.CollisionShape)) yield return new Contact(a, mainBody, b);
                
            }
        }
    }
}
