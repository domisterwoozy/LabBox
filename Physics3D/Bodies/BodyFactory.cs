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
                EMNone.Instance,
                new BasicMaterial(),
                Point.Instance,
                Point.Instance,
                PointBound.Instance);
        }

        public static BasicBody SphereMass(double radius, double mass, Vector3 pos, Vector3 vel)
        {
            return new BasicBody(
                new RigidBody6DOF(
                    new EuclideanKinematics(new Transform(pos, Matrix3.Identity), vel, Vector3.Zero),
                    mass,
                    Matrix3.Identity),
                EMNone.Instance,
                new BasicMaterial(),
                new SphereIntersectorVolume(Vector3.Zero, radius, 5),
                new SphereIntersectorVolume(Vector3.Zero, radius, 5),
                new SphereBound(radius));
        }

        public static BasicBody Cuboid(double length, double width, double height, Vector3 pos, Vector3 vel = default(Vector3), Vector3 omega = default(Vector3))
        {
            return new BasicBody(
                new RigidBody6DOF(
                    new EuclideanKinematics(new Transform(pos, Matrix3.Identity), vel, omega),
                    1.0,
                    Matrix3.Identity),
                EMNone.Instance,
                new BasicMaterial(),
                Intersectors.CuboidIntersector(length, width, height),
                new Cuboid(length, width, height),
                new SphereBound(new Vector3(length / 2, width / 2, height / 2).Magnitude));               
        }

        public static IEnumerable<BasicBody> Box(double length, double width, double height, Vector3 center)
        {
            yield return Cuboid(length, width, 1.0, new Vector3(0, 0, -height / 2)); // bottom
            yield return Cuboid(length, width, 1.0, new Vector3(0, 0, height / 2)); // top

            yield return Cuboid(length, 1.0, height, new Vector3(0, -width / 2, 0)); // front
            yield return Cuboid(length, 1.0, height, new Vector3(0, width / 2, 0)); // back

            yield return Cuboid(1.0, width, height, new Vector3(-length / 2, 0, 0)); // left
            yield return Cuboid(1.0, width, height, new Vector3(length / 2, 0, 0)); // right

        }


    }
}
