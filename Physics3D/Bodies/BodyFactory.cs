using Math3D;
using Math3D.Geometry;
using Physics3D.Dynamics;
using Physics3D.ElectroMagnetism;
using Physics3D.Kinematics;
using Physics3D.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Bodies
{
    public static class BodyFactory
    {
        public static BasicBody PointMass(double mass, Vector3 pos, Vector3 vel)
        {
            return new BasicBody(
                new RigidBody6DOF(
                    new EuclideanKinematics(new Transform(pos, Matrix3.Identity), vel, Vector3.Zero),
                    mass,
                    Matrix3.Identity),
                None.Instance,
                new BasicMaterial(),
                Point.Instance,
                NeverOverlap.Instance);
        }

        public static BasicBody SphereMass(double radius, double mass, Vector3 pos, Vector3 vel)
        {
            return new BasicBody(
                new RigidBody6DOF(
                    new EuclideanKinematics(new Transform(pos, Matrix3.Identity), vel, Vector3.Zero),
                    mass,
                    Matrix3.Identity),
                None.Instance,
                new BasicMaterial(),
                new SphereColliderVolume(Vector3.Zero, radius, 30),
                new SphereBound(radius));
        }

        public static BasicBody Wall(double x, double y, double z, Vector3 pos)
        {
            return new BasicBody(
                new RigidBody6DOF(
                    new EuclideanKinematics(new Transform(pos, Matrix3.Identity), Vector3.Zero, Vector3.Zero),
                    double.PositiveInfinity,
                    Matrix3.Identity),
                None.Instance,
                new BasicMaterial(),
                new CuboidColliderVolume(x, y, z, pos),
                AlwaysOverlap.Instance);
        }


    }
}
