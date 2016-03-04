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
                .Where(otherBody => mainBody.BoundVolume.AreOverlapping(otherBody.BoundVolume, mainBody.Dynamics.Transform.Pos, otherBody.Dynamics.Transform.Pos)) // bound volume check
                .SelectMany(otherBody => ContactsOnFirst(otherBody, mainBody).Union(ContactsOnFirst(otherBody, mainBody))); // full collider intersection check   
        }

        // all of the contacts where second's edges intersect first's body
        // handles all cordinate transformation from each body to the world
        private IEnumerable<Contact> ContactsOnFirst(IBody first, IBody second)
        {
            Transform firstTransform = first.Dynamics.Transform;
            Transform secondTransform = second.Dynamics.Transform;
            var secondToFirst = new Transformation(secondTransform, firstTransform);        

            return
                // everything is converted to the firsts local transform coords
                second.CollisionShape.OuterEdges.SelectMany(secondEdge => first.CollisionShape.FindIntersections(secondToFirst.TransformEdge(secondEdge)) 
                .Select(i => new Contact(firstTransform.ToWorldSpace(i), second, first))); // then to world coords
        }
            
    }
}
