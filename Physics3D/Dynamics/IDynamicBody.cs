using Math3D;
using Math3D.Geometry;
using Physics3D.Kinematics;
using Physics3D.Universes;
using System;
using System.Collections.Generic;

namespace Physics3D.Dynamics
{
    /// <summary>
    /// An interface for interacting with a kinematic body. Allows a client to enact external
    /// forces onto a kinematic body and encapsulates the state changes that occur from these forces.
    /// </summary>
    public interface IDynamicBody
    {
        event EventHandler<FrameLengthArgs> FrameFinished;

        #region Getters
        /// <summary>
        /// The underlying 3D state of the body.
        /// </summary>
        IKinematics Kinematics { get; }

        /// <summary>
        /// Convienent shortcut to Kinematics.Transform
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// The total kinetic energy of the body. Includes translational and rotational energy.
        /// </summary>
        double KineticEnergy { get; }

        /// <summary>
        /// Linear momentum of the body.
        /// </summary>
        Vector3 P { get; }

        /// <summary>
        /// Angular momentum of the body
        /// </summary>
        Vector3 L { get; }

        /// <summary>
        /// The mass of the object. If the object is immovable than this value is positive infinity.
        /// </summary>
        double Mass { get; }

        /// <summary>
        /// The multiplicative inverse of the mass of this object. If the object is immovable than this value is zero.
        /// </summary>
        double InvMass { get; }

        /// <summary>
        /// The inertia tensor of this body. This value might not be valid if axis are fixed.
        /// </summary>
        Matrix3 I { get; }

        /// <summary>
        /// The inverse of the inertia tensor. 
        /// </summary>
        Matrix3 InvI { get; }

        /// <summary>
        /// Whether this body is completely immovable (rotation and translation).
        /// </summary>
        bool IsFixed { get; }

        /// <summary>
        /// Whether the bodys position is fixed.
        /// </summary>
        bool IsPositionFixed { get; }

        /// <summary>
        /// Whether the rotation of this body is fixed.
        /// </summary>
        bool IsRotationFixed { get; }

        /// <summary>
        /// Whether rotation around a particular axis is fixed.
        /// </summary>
        bool IsAxisFixed(Axis axis);
        #endregion

        #region Setters
        /// <summary>
        /// Updates the state of this body by one physics frame.
        /// </summary>
        void Update(double deltaTime);

        /// <summary>
        /// Adds a permanent force and torque to the center of mass of the body. Inputs should be in world coordinates.
        /// </summary>
        void AddInputs(Vector3 force, Vector3 torque);

        /// <summary>
        /// Adds a temporary force and torque to the center of mass of the body for a specified duration.
        /// Inputs should be in world coordinates.
        /// </summary>
        void ThrustInputs(Vector3 force, Vector3 torque, float duration);

        /// <summary>
        /// Adds a temporary force and torque to the center of mass of this body that will occur for only the next physics frame.
        /// </summary>
        void ThrustSingleFrame(Vector3 force, Vector3 torque);
        
        /// <summary>
        /// Enacts an isntantaneous impulse on the body. Will change the momentums before the next physics frame occurs.
        /// </summary>
        /// <param name="impulse">Impulse in world coordaintes</param>
        /// <param name="relPos">Vector from the center of mass to the location of the impulse. In world coordinates.</param>
        void EnactImpulse(Vector3 impulse, Vector3 relPos);

        /// <summary>
        /// Locks the center of mass of the body to a manifold. The body must initially be on the manifold.
        /// </summary>
        void ConstraintToManifold(IManifold manifold);

        /// <summary>
        /// Completely fixes the rotation and position of this body.
        /// </summary>
        void Fix();

        /// <summary>
        /// Fixes the rotation of this body.
        /// </summary>
        void FixRotation();

        /// <summary>
        /// Fixes the position of this body.
        /// </summary>
        void FixPosition();

        /// <summary>
        /// Fixes rotation around a set of axis. Supplying a set of all 3 axis is the same as calling FixRotation().
        /// </summary>
        void FixAxes(ISet<Axis> axes);

        /// <summary>
        /// Prevents all rotation except around a certain specified axis.
        /// </summary>
        /// <param name="axisBody">The axes of rotation in body coordinates.</param>
        /// <param name="scalarInertia">The amount of inertia around the specified axis.</param>
        void FixRotAroundAxis(Vector3 axisBody, double scalarInertia);
        #endregion
    }
}
