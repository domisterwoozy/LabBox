using System;
using System.Collections.Generic;
using Math3D;
using Math3D.VectorCalc;
using Physics3D.Dynamics;

namespace Physics3D.Universes
{
    /// <summary>
    /// A universe that simulates the two-body central-force problem.
    /// </summary>
    public class TwoBodyUniverse// : IUniverse
    {
        public double GravitationalConstant { get; private set; } = 6.67408 * Math.Pow(10, -11);
        public IDynamicBody BodyOne { get; }
        public IDynamicBody BodyTwo { get; }
        public IScalarField GravitationalPotential { get; }
        public ICollection<IDynamicBody> DynamicBodies => new[] { BodyOne, BodyTwo };
        public ICollection<IVectorField> ForceFields => new IVectorField[0];
        public ICollection<IScalarField> Potentials => new[] { GravitationalPotential };

        // absolute constants        
        public double TotalMass { get; }
        public double ReducedMass { get; }
        public double SemiLatusRectum { get; }
        public bool IsBound { get; }
        public double SemiMajorAxis { get; }
        public Vector3 TotalAngularMomentum { get; }
        public double TotalEnergy { get; }
        public Vector3 CenterOfMassVel { get; }
        public double Eccentricity { get; }

        // vary over time
        public Vector3 RelativePosition => BodyOne.Transform.Pos - BodyTwo.Transform.Pos;
        public Vector3 CenterOfMass => (1.0 / TotalMass) * (BodyOne.Mass * BodyOne.Transform.Pos + BodyTwo.Mass * BodyTwo.Transform.Pos);
        public double Legrangian => BodyTwo.KineticEnergy + BodyTwo.KineticEnergy - GravitationalPotential.Value(RelativePosition);
        public double TotalKineticEnergy => BodyOne.KineticEnergy + BodyTwo.KineticEnergy;
        public Vector3 RelativePositionVel { get; private set; }

        // the next two properties come from the equation 8.12 in Classical Mechanics (Taylor)
        // T = 0.5 * Total Mass * CenterOfMassVel^2 + 0.5 * ReducedMass * RelativePosVel^2

        /// <summary>
        /// The kinetic energy of a fictitious particle of mass 'Total Mass' moving with the same speed as the center of mass of the system.
        /// </summary>
        public double CenterOfMassEnergy => 0.5 * TotalMass * Math.Pow(CenterOfMassVel.Magnitude, 2);
        /// <summary>
        /// The kinetic energy of a fictitious particle of mass 'Reduced Mass' moving with the speed of the relative position vector.
        /// </summary>
        public double ReducedMassEnergy => TotalKineticEnergy - CenterOfMassEnergy;


        public TwoBodyUniverse(IDynamicBody bodyOne, IDynamicBody bodyTwo, double gravConstant)
        {
            BodyOne = bodyOne;
            BodyTwo = bodyTwo;
            GravitationalConstant = gravConstant;

            // initialzie constants
            TotalMass = BodyOne.Mass + BodyTwo.Mass;
            ReducedMass = (BodyOne.Mass * BodyTwo.Mass) / TotalMass;
            //SemiLatusRectum = TotalMomentum.MagSquared / (ReducedMass * GravitationalConstant * BodyOne.Mass * BodyTwo.Mass);
            //TotalMomentum = BodyOne.P + BodyTwo.P;
            //CenterOfMassVel = (1.0 / TotalMass) * TotalMomentum;          
            TotalEnergy = TotalKineticEnergy + GravitationalPotential.Value(RelativePosition);
            //Eccentricity = Math.Sqrt(1 + (2 * TotalEnergy * TotalMomentum.MagSquared) / (ReducedMass * Math.Pow(GravitationalConstant * BodyOne.Mass * BodyTwo.Mass, 2)));
            IsBound = TotalEnergy < 0.0;
            SemiMajorAxis = SemiLatusRectum / (1 - Math.Pow(Eccentricity, 2));
        }  

     
        
        public double GetRadiusAtAngle(double angle)
        {
            double c = TotalAngularMomentum.MagSquared / (ReducedMass * GravitationalConstant * BodyOne.Mass * BodyTwo.Mass);
            return c / (1 + Eccentricity * Math.Cos(angle));
        }


        
        




        
        public void Update(double deltaTime)
        {
            
        }
    }
}
