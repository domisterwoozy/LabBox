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
        public StrategyContainer<IEdgeIntersector, Transformation, IEnumerable<Intersection>> IntersectStrats { get; }
        public StrategyContainer<IOverlapable, Transformation, bool> OverlapStrats { get; }

        public BasicContactFinder()
        {
            IntersectStrats = new StrategyContainer<IEdgeIntersector, Transformation, IEnumerable<Intersection>>();
            OverlapStrats = new StrategyContainer<IOverlapable, Transformation, bool>();

            // add some default strategies
            IntersectStrats.AddStrategy(new SphereSphereStrategy());
            IntersectStrats.AddStrategy(new SphereCompositeStrategy());            
            OverlapStrats.AddStrategy(new SphereSphereOverlap());

        }

        public IEnumerable<Contact> FindContacts(IBody mainBody, IEnumerable<IBody> otherBodies)
        {
            return otherBodies
                .Where(otherBody => otherBody != mainBody) // cannot contact self
                .Where(otherBody => !OverlapStrats.HasStrategy(mainBody.BoundVolume, otherBody.BoundVolume) // where we dont have an overlap strategy
                     || OverlapStrats.EnactStrategy(mainBody.BoundVolume, otherBody.BoundVolume, // or the overlap strategy passes (they are overlapping)
                     new Transformation(mainBody.Dynamics.Transform, otherBody.Dynamics.Transform))) 
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

            if (IntersectStrats.HasStrategy(firstShape, secondShape))
            {
                return IntersectStrats.EnactStrategy(firstShape, secondShape, secondToFirst.Reverse()).Select(i => new Contact(i, first, second));
            }

            // general case             
            return
                // everything is converted to the firsts local transform coords
                secondShape.Edges.SelectMany(secondEdge => firstShape.FindIntersections(secondToFirst.TransformEdge(secondEdge)) 
                .Select(i => new Contact(firstTransform.ToWorldSpace(i), first, second))); // then to world coords
        }
    }


}
