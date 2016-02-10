using Math3D;

namespace Physics3D.Kinematics
{
    /// <summary>
    /// An interface that represents how a transform changes over time. This describes the "geometry of motion".
    /// Different implementations of this interface represent different spacetime metrics.
    /// </summary>
    public interface IKinematicBody
    {
        /// <summary>
        /// The local coordinate system of this kinematic body.
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// The instantaneous linear velocity of the center of mass of this body in world coordinates.
        /// </summary>
        Vector3 V { get; set; }

        /// <summary>
        /// The angular velocity of the body in world coordinates.
        /// </summary>
        Vector3 Omega { get; set; }

        /// <summary>
        /// The instantaneous linear velocity of a point on this bodies surface.
        /// </summary>
        /// <param name="p">A vector from the center of mass of this body to the point in question.</param>
        Vector3 SurfaceVelocity(Vector3 p);

        /// <summary>
        /// Updates the transform state by integrating over a specified time step.
        /// </summary>
        void UpdateTransform(double deltaTime);

        IKinematics GetCurrentState();
    }

    /// <summary>
    /// An immutable snapshot of an IKinematicBody.
    /// </summary>
    public interface IKinematics
    {
        /// <summary>
        /// The local coordinate system of this kinematic body.
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// The instantaneous linear velocity of the center of mass of this body in world coordinates.
        /// </summary>
        Vector3 V { get; }

        /// <summary>
        /// The angular velocity of the body in world coordinates.
        /// </summary>
        Vector3 Omega { get; }

        /// <summary>
        /// The instantaneous linear velocity of a point on this bodies surface.
        /// </summary>
        /// <param name="p">A vector from the center of mass of this body to the point in question.</param>
        Vector3 SurfaceVelocity(Vector3 p);
    }    
}
