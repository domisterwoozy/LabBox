using Math3D;
using Math3D.VectorCalc;
using Physics3D.Bodies;
using Physics3D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Physics3D.Forces
{
    public delegate Vector3 ForceApplicationFunc(IBody body, Vector3 rawForce);

    /// <summary>
    /// Nearly all forces are not completely independent of the body they are acting on (ex: classical gravity, electromagnetism, drag, etc)
    /// This class allows the ability to apply a basic vector force field to a body while considering properties of the body besides position.
    /// (ex: mass, velocity, charge, etc).
    /// </summary>
    public class ForceField
    {
        public IVectorField RawField { get; }
        public ForceApplicationFunc ForceApplicationFunc { get; }
        public ForceApplicationFunc TorqueApplicationFunc { get; }

        public ForceField(IVectorField rawField) : this(rawField, (body, force) => force, (body, torque) => torque) { }

        public ForceField(IVectorField rawField, ForceApplicationFunc applFunc) : this(rawField, applFunc, (body, torque) => torque) { }

        public ForceField(IVectorField rawField, ForceApplicationFunc forApplFunc, ForceApplicationFunc torqueApplFunc)
        {
            RawField = rawField;
            ForceApplicationFunc = forApplFunc;
            TorqueApplicationFunc = torqueApplFunc;
        }

        public Vector3 GetForceOnBody(IBody body) => ForceApplicationFunc(body, RawField.Value(body.Dynamics.Transform.Pos));
        public Vector3 GetTorqueOnBody(IBody body) => TorqueApplicationFunc(body, RawField.Value(body.Dynamics.Transform.Pos));
    }

    public static class ForceFields
    {
        #region Application Functions
        /// <summary>
        /// F = |V|^2 * cd * area * W
        /// </summary>
        public static Vector3 DragForceApplier(IBody body, Vector3 windField)
        {
            double area = body.CollisionShape.CrossSectionalArea(body.Dynamics.Transform.Pos, body.Dynamics.Kinematics.V);
            return body.Dynamics.Kinematics.V.MagSquared  * area * body.Material.DragCoef * windField;
        }
        /// <summary>
        /// F = m * G
        /// </summary>
        public static Vector3 GravityForceApplier(IBody body, Vector3 gravField) => body.Dynamics.Mass * gravField;
        /// <summary>
        /// F = q * E
        /// </summary>
        public static Vector3 ElectricForceApplier(IBody body, Vector3 electricField) => body.EMProps.Charge * electricField;
        /// <summary>
        /// T = p X E
        /// </summary>
        public static Vector3 ElectricTorqueApplier(IBody body, Vector3 electricField) => body.EMProps.ElectricDipoleMoment ^ electricField;
        /// <summary>
        /// F = q * (V X B)
        /// </summary>
        public static Vector3 MagnetismForceApplier(IBody body, Vector3 magneticField) => body.EMProps.Charge * (body.Dynamics.Kinematics.V ^ magneticField);
        /// <summary>
        /// T = m X B
        /// </summary>
        public static Vector3 MagnetismTorqueApplier(IBody body, Vector3 magneticField) => body.EMProps.MagneticDipoleMoment ^ magneticField;
        #endregion

        #region Fields
        /// <summary>
        /// A basic force field that has no dependence on the object the force is acting on.
        /// </summary>
        public static ForceField BasicForce(IVectorField forceField) => new ForceField(forceField);
        public static ForceField Drag(IVectorField windField) => new ForceField(windField, DragForceApplier);

        public static ForceField Gravity(IDynamicBody sourceBody, double gravConstant) => Gravity(() => sourceBody.Transform.Pos, sourceBody.Mass * gravConstant);
        public static ForceField Gravity(Generator<Vector3> posGen, double strength)
        {
            return new ForceField(PhysicsFields.PointMassGravity(strength).Translate(posGen), GravityForceApplier);
        }

        public static ForceField ElectricCharge(IBody sourceBody, double electricConstant) => ElectricCharge(() => sourceBody.Dynamics.Transform.Pos, sourceBody.EMProps.Charge, electricConstant);
        public static ForceField ElectricCharge(Generator<Vector3> posGen, double charge, double electricConstant)
        {            
            return new ForceField(PhysicsFields.PointChargeElectric(charge,  electricConstant).Translate(posGen), ElectricForceApplier, ElectricTorqueApplier);
        }

        public static ForceField ElectricDipole(IBody sourceBody, double electricConstant) =>
            ElectricDipole(() => sourceBody.Dynamics.Transform.Pos, sourceBody.EMProps.ElectricDipoleMoment, electricConstant);
        public static ForceField ElectricDipole(Generator<Vector3> posGen, Vector3 elecMoment, double electricConstant)
        {
            return new ForceField(PhysicsFields.ElectricDipole(elecMoment, electricConstant).Translate(posGen), ElectricForceApplier, ElectricTorqueApplier);
        }

        public static ForceField MagneticDipole(IBody sourceBody, double magneticConstant)
            => MagneticDipole(() => sourceBody.Dynamics.Transform.Pos, sourceBody.EMProps.MagneticDipoleMoment, magneticConstant);        
        public static ForceField MagneticDipole(Generator<Vector3> posGen, Vector3 magMoment, double magneticConstant)
        {
            return new ForceField(PhysicsFields.MagneticDipole(magMoment, magneticConstant).Translate(posGen), MagnetismForceApplier, MagnetismTorqueApplier);
        }
        #endregion
    }
}
