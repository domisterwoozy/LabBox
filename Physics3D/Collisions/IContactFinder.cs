using Math3D;
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
            return otherBodies
                .Where(otherBody => otherBody != mainBody) // cannot contact self
                .Where(otherBody => mainBody.BoundVolume.AreOverlapping(otherBody.BoundVolume)) // bound volume check
                .SelectMany(otherBody => ContactsOnFirst(mainBody, otherBody).Union(ContactsOnFirst(otherBody, mainBody))); // full collider intersection check   
        }

        // all of the contacts where second's edges intersect first's body
        private IEnumerable<Contact> ContactsOnFirst(IBody first, IBody second)
        {
            Transform firstTransform = first.Dynamics.Transform;
            Transform secondTransform = second.Dynamics.Transform;
            // everything is converted to the firsts local transform coords

            return 
                second.CollisionShape.OuterEdges.SelectMany(secondEdge => second.CollisionShape.FindIntersections(secondEdge.TransformCoords(secondTransform, firstTransform))
            .Select(i => new Contact(i, first, second)));
        }
            
    }
}
