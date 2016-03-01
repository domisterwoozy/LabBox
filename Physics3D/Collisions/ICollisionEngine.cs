﻿using Math3D;
using Math3D.Geometry;
using Physics3D.Bodies;
using Physics3D.Kinematics;
using Physics3D.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Collisions
{

    public interface ICollisionEngine
    {
        /// <summary>
        /// Determines the impulse generated by the intersection between two bodies.
        /// The intersection normal should be pointing away from body B and towards body A.
        /// The returned impulse should be enacted on body A and the negation enacted on body B.
        /// The location of the impulse is the location of the intersection in world space.
        /// </summary>
        Vector3 Collide(Intersection intersection, IKinematics kinematicsA, IKinematics kinematicsB, IMaterial matA, IMaterial matB);
    }

    public class CollisionEngine : ICollisionEngine
    {
        /// <summary>
        /// Objects colliding with a relative velocity below this number are not considered colliding.
        /// </summary>
        public double RestingThresholdSpeed { get; set; } = 0.01f;

        /// <summary>
        /// A factor of global 'bounciness'
        /// </summary>
        public double Epsilon { get; set; } = 0.05f;


        public Vector3 Collide(Intersection intersection, IBody a, IBody b)
        {
            // IMPLEMENTATION EXPLANATION           
            // point p is the collision point
            // vector n is the collision normal and it defines the 'collision plane'
            // the relative velocity vector and n are in another plane called the 'parallel plane" and it is defined by a vector called nPerp (vRel ^ n = nPerp)
            // point a is the cm of body a
            // point b is the cm of body b
            // imagine the collision occuring in the frame where body b is fixed

            IKinematics kinematicsA = a.Dynamics.Kinematics;
            IKinematics kinematicsB = b.Dynamics.Kinematics;

            Vector3 aToP = intersection.Point - kinematicsA.Transform.Pos;
            Vector3 bToP = intersection.Point - kinematicsB.Transform.Pos;
            // the velocity of p relative to both bodies
            Vector3 aVelP = kinematicsA.SurfaceVelocity(aToP);
            Vector3 bVelP = kinematicsB.SurfaceVelocity(bToP);

            Vector3 vRel = aVelP - bVelP; // the relative velocity of the colliding points
            double vRelPerp = vRel * intersection.Normal.Inverse; // negative means A is moving towards the plane (colliding)
            if (vRelPerp > -RestingThresholdSpeed) return Vector3.Zero; // either receding or resting on eachother

            Vector3 normalImpulse = NormalImpulse(vRelPerp, a, b, aToP, bToP, intersection.Normal) * intersection.Normal.Inverse;
            Vector3 frictionImpulse = Vector3.Zero;

            Vector3 nPerp = vRel ^ intersection.Normal; // will be zero if vRel is completely normal to collision plane and therefore no friction component
            if (nPerp.MagSquared != 0) // there is motion in collision plane -> friction exists
            {
                // the normalized projectio0n of vRel onto the collision plane
                Vector3 nPar = (intersection.Normal ^ nPerp).UnitDirection; // defines the collision plane
                double vRelPar = vRel * nPar;
                double fric = FrictionImpulse(normalImpulse.Magnitude, vRelPar);
                double maxFric = MaxFrictionImpulse(vRelPar, nPar, normalImpulse.Magnitude);
                if (Math.Abs(fric) > Math.Abs(maxFric)) fric = maxFric;
                frictionImpulse = fric * nPar.Inverse; 
            }

            return frictionImpulse + normalImpulse;
        }

        /// <summary>
        /// Calculates the impulse that would create zero relative parallel velocity b/w the two bodies.
        /// All calculations occur in one plane. The plane is defined by the plane that nPar and vRel exist in and I am calling it the parallel plane.
		/// Reducing everything to one plane allows you to simplify the cross product and find a unique result value.
        /// </summary>
        private double MaxFrictionImpulse(double vRelPar, Vector3 nPar, double j)
        {
            throw new NotImplementedException();
        }
        private double FrictionImpulse(double j, double vRelPar)
        {
            throw new NotImplementedException();
        }

        private double NormalImpulse(double vRelPerp, IBody a, IBody b, Vector3 aToP, Vector3 bToP, Vector3 n)
        {
            double num = 1 + DerivedEpsilon(a.Material, b.Material) * Math.Abs(vRelPerp);
            double term1 = a.Dynamics.InvMass;
            double term2 = b.Dynamics.InvMass;
            double term3 = n * (a.Dynamics.InvI * (aToP ^ n) ^ aToP);
            double term4 = n * (b.Dynamics.InvI * (bToP ^ n) ^ bToP);
            return num / (term1 + term2 + term3 + term4);
        }

        private double DerivedEpsilon(IMaterial a, IMaterial b) => Epsilon * (a.Epsilon + b.Epsilon) / 2;
        

    }
}
