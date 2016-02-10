using Math3D;
using Physics3D.Kinematics;

namespace Physics3D.Kinematics
{
    public class EuclideanKinematicBody : IKinematicBody
    {
        public Transform Transform { get; private set; }

        public Vector3 V { get; set; }

        public Vector3 Omega { get; set; }

        public EuclideanKinematicBody(Transform initialTransform)
        {
            Transform = initialTransform;
        }

        public IKinematics GetCurrentState() => new EuclideanKinematics(Transform, V, Omega);

        public Vector3 SurfaceVelocity(Vector3 p) => V + Omega ^ p;

        public void UpdateTransform(double deltaTime)
        {
            Transform = Transform.Translate(deltaTime * V);
            // dq/dt = qdot = 0.5 * w(t) * q(t) *** Appendix B of David Barraff Physically Based Modeling - Rigid Body Simulation
            // dq = deltaTime * dq/dt;
            Quaternion dq = deltaTime * 0.5 * Quaternion.VectorQ(Omega) * Transform.Q;
            Transform = new Transform(Transform.Pos, Transform.Q + dq);
        }

    }
}
