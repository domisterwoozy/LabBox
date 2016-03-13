using Math3D;
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
    public class ImpulseCollisionEngine : IImpulseEngine
    {
        /// <summary>
        /// Global coefficient of static friction.
        /// </summary>
        public double StaticFrictionFactor { get; set; } = 2.0;

        /// <summary>
        /// Global coefficient of dynamic friction.
        /// </summary>
        public double DynamicFrictionFactor { get; set; } = 0.5f;

        /// <summary>
        /// A factor of global 'bounciness'
        /// </summary>
        public double Epsilon { get; set; } = 1.0f;

        /// <summary>
        /// Objects receding with a relative velocity lower than this are still considered colliding.
        /// Raise this if you have strong forces that are causing objects to interpenetrate while 'resting'
        /// </summary>
        public double CollidingThresholdSpeed { get; set; } = 0.1f;

        /// <summary>
        /// Objects colliding with a relative paralell velocity below this threshold experience static friction.
        /// </summary>
        public double StaticFrictionThresholdSpeed { get; set; } = 0.01f;


        public Vector3 Collide(Contact c)
        {
            /// IMPLEMENTATION EXPLANATION           
            /// point p is the collision point
            /// vector n is the collision normal and it defines the 'collision plane'
            /// the relative velocity vector and n are in another plane called the 'parallel plane" and it is defined by a vector called nPerp (vRel % n = nPerp)
            /// point a is the cm of body a
            /// point b is the cm of body b
            /// imagine the collision occuring in the frame where body b is fixed

            IBody a = c.BodyA;
            IBody b = c.BodyB;
            Intersection intersection = c.Intersection;

            IKinematics kinematicsA = a.Dynamics.Kinematics;
            IKinematics kinematicsB = b.Dynamics.Kinematics;

            Vector3 aToP = intersection.Point - kinematicsA.Transform.Pos;
            Vector3 bToP = intersection.Point - kinematicsB.Transform.Pos;
            // the velocity of p relative to both bodies
            Vector3 aVelP = kinematicsA.SurfaceVelocity(aToP);
            Vector3 bVelP = kinematicsB.SurfaceVelocity(bToP);

            Vector3 vRel = aVelP - bVelP; // the relative velocity of the colliding points
            double vRelPerp = vRel * intersection.Normal.Inverse; // negative means A is moving towards the plane (colliding)
            if (vRelPerp > CollidingThresholdSpeed) return Vector3.Zero; // objects are receding

            double scalarNormalImpulse = NormalImpulse(vRelPerp, a, b, aToP, bToP, intersection.Normal);
            Vector3 normalImpulse = scalarNormalImpulse * intersection.Normal.Inverse;
            Vector3 frictionImpulse = Vector3.Zero;
            
            Vector3 nPerp = (vRel % intersection.Normal).UnitDirection; // will be zero if vRel is completely normal to collision plane and therefore no friction component
            if (nPerp.MagSquared != 0) // there is motion in collision plane -> friction exists
            {
                // the normalized projectio0n of vRel onto the collision plane
                // it lies in the collision plane and defines the paralell plane
                Vector3 nPar = (intersection.Normal % nPerp).UnitDirection;
                double vRelPar = vRel * nPar;
                double fric = FrictionImpulse(scalarNormalImpulse, vRelPar, a.Material, b.Material);
                double maxFric = MaxFrictionImpulse(a, b, vRelPar, nPar, nPerp, scalarNormalImpulse, aToP, bToP);
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
        private double MaxFrictionImpulse(IBody a, IBody b, double vRelPar, Vector3 nPar, Vector3 nPerp, double j, Vector3 aToP, Vector3 bToP)
        {
            double c2a = 0;
            double c2b = 0;
            double thetaA;
            double thetaB;
            Vector3 apPar;
            Vector3 bpPar;

            if (!a.Dynamics.IsFixed)
            {
                apPar = aToP.ProjectToPlane(nPerp);
                thetaA = nPar.AngleBetween(apPar.Inverse);
                Vector3 axisOfRotA = (nPar % apPar).UnitDirection;
                double invScalarRotInertiaA = a.Dynamics.InvI.Magnitude(axisOfRotA);
                c2a = Math.Pow(apPar.Magnitude, 2) * Math.Sign(thetaA) * Math.Cos(thetaA + Math.PI / 2) * invScalarRotInertiaA - a.Dynamics.InvMass;
            }
            if (!b.Dynamics.IsFixed)
            {
                bpPar = bToP.ProjectToPlane(nPerp);
                thetaB = nPar.AngleBetween(bpPar.Inverse);
                Vector3 axisOfRotB = (nPar % bpPar).UnitDirection;
                double invScalarRotInertiaB = b.Dynamics.InvI.Magnitude(axisOfRotB);
                c2b = Math.Pow(bpPar.Magnitude, 2) * Math.Sign(thetaB) * Math.Cos(thetaB + Math.PI / 2) * invScalarRotInertiaB - b.Dynamics.InvMass;
            }
            double result = vRelPar / (-c2b - c2a);
            // debugging
            if (double.IsNaN(result) || double.IsInfinity(result)) throw new InvalidOperationException("Friction cannot be NaN");
            if (result < 0) return 0;//  throw new InvalidOperationException("Friction cannot be negative");

            return result;
        }

        private double FrictionImpulse(double normalImpulse, double vRelPar, IMaterial a, IMaterial b)
        {
            // might want to make this condition depend on the materials as well
            if (Math.Abs(vRelPar) < StaticFrictionThresholdSpeed) return DerivedStaticCoef(a, b) * normalImpulse;
            else return DerivedDynamicCoef(a, b) * normalImpulse;
        }

        private double NormalImpulse(double vRelPerp, IBody a, IBody b, Vector3 aToP, Vector3 bToP, Vector3 n)
        {
            double num = (1 + DerivedEpsilon(a.Material, b.Material)) * Math.Pow(Math.Abs(vRelPerp), 1.0);
            double term1 = a.Dynamics.InvMass;
            double term2 = b.Dynamics.InvMass;
            double term3 = n * (a.Dynamics.InvI * (aToP % n) % aToP);
            double term4 = n * (b.Dynamics.InvI * (bToP % n) % bToP);
            return num / (term1 + term2 + term3 + term4);
        }

        private double DerivedDynamicCoef(IMaterial a, IMaterial b) => DynamicFrictionFactor * ((a.DynamicFrictionCoef + b.DynamicFrictionCoef) / 2);
        private double DerivedStaticCoef(IMaterial a, IMaterial b) => StaticFrictionFactor * ((a.StaticFrictionCoef + b.StaticFrictionCoef) / 2);
        private double DerivedEpsilon(IMaterial a, IMaterial b) => Epsilon * ((a.Epsilon + b.Epsilon) / 2);

        private static void Validate(Vector3 v)
        {
            Validate(v.X);
            Validate(v.Y);
            Validate(v.Z);
        }

        private static void Validate(double d)
        {
            if (double.IsNaN(d)) throw new ArgumentException("Input must not be NaN or Infinity");
            if (double.IsInfinity(d)) throw new ArgumentException("Input must not be NaN or Infinity");
        }

    }
}
