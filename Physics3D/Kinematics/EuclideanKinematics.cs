using Math3D;

namespace Physics3D.Kinematics
{
    /// <summary>
    /// Represents the instantaneous state of a euclidean body.
    /// Todo: Might want to change to a struct after performance testing.
    /// </summary>
    public class EuclideanKinematics : IKinematics
    {
        public static readonly IKinematics RestingKinematics = new EuclideanKinematics(Transform.Zero, Vector3.Zero, Vector3.Zero);

        public Transform Transform { get; }

        public Vector3 V { get; }

        public Vector3 Omega { get; }

        public EuclideanKinematics(Transform t, Vector3 v, Vector3 omega)
        {
            Transform = t;
            V = v;
            Omega = omega;
        }

        public Vector3 SurfaceVelocity(Vector3 p) => V + Omega % p;
    }
}
