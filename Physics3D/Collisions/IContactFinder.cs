using Math3D;
using Math3D.Geometry;
using Physics3D.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

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
                .SelectMany(otherBody => ContactsOnFirst(otherBody, mainBody).Union(ContactsOnFirst(mainBody, otherBody))); // full collider intersection check   
        }

        // all of the contacts where second's edges intersect first's body
        // handles all cordinate transformation from each body to the world
        private IEnumerable<Contact> ContactsOnFirst(IBody first, IBody second)
        {
            IEdgeIntersector firstShape = first.CollisionShape;
            IEdgeIntersector secondShape = second.CollisionShape;
            Transform firstTransform = first.Dynamics.Transform;
            Transform secondTransform = second.Dynamics.Transform;
            var secondToFirst = new Transformation(secondTransform, firstTransform);

            // first special case (sphere against sphere)
            var firstSphere = firstShape as SphereIntersectorVolume;
            var secondSphere = secondShape as SphereIntersectorVolume;
            if (firstSphere != null && secondSphere != null)
            {
                Intersection? inter = ContactsOnFirst(firstSphere.Radius, secondSphere.Radius, firstTransform.Pos, secondTransform.Pos);
                if (!inter.HasValue) return Enumerable.Empty<Contact>();
                return MyExtensions.EnumerableOf(new Contact(inter.Value, first, second));
            }
            //// more special cases go here
            //if (secondSphere != null)
            //{
            //    // the edges are coming from the sphere so we do some approximations since spheres dont have enough edges
            //    Intersection[] inters = secondSphere.Edges.SelectMany(secondEdge => firstShape.FindIntersections(secondToFirst.TransformEdge(secondEdge))).Select(i => firstTransform.ToWorldSpace(i)).ToArray(); // in world space
            //    if (inters.Length == 0) return Enumerable.Empty<Contact>();
            //    // average the location of the contacts and then project it to the sphere
            //    Vector3 worldContactPoint = inters.Average(i => i.Point);
            //    Vector3 sphereWorldPoint = second.Dynamics.Transform.Pos;
            //    worldContactPoint = sphereWorldPoint + secondSphere.Radius * (worldContactPoint - sphereWorldPoint).UnitDirection;
            //    return MyExtensions.EnumerableOf(new Contact(new Intersection(worldContactPoint, -(worldContactPoint - sphereWorldPoint).UnitDirection), first, second));
            //}

            // general case             
            return
                // everything is converted to the firsts local transform coords
                secondShape.Edges.SelectMany(secondEdge => firstShape.FindIntersections(secondToFirst.TransformEdge(secondEdge)) 
                .Select(i => new Contact(firstTransform.ToWorldSpace(i), first, second))); // then to world coords
        }




        // Special case of two intersecting spheres
        private Intersection? ContactsOnFirst(double firstRadius, double secondRadius, Vector3 firstPos, Vector3 secondPos)
        {
            Vector3 firstToSecond = secondPos - firstPos;
            double totalRadius = firstRadius + secondRadius;
            if (firstToSecond.Magnitude > totalRadius) return null;
            return new Intersection(firstPos + firstToSecond * (firstRadius / totalRadius), firstToSecond.UnitDirection);
        }
    }


}
