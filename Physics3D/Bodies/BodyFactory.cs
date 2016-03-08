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
                NeverOverlap.Instance,
                Point.Instance);
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
                new SphereIntersectorVolume(Vector3.Zero, radius, 10),
                new SphereBound(radius),
                new SphereIntersectorVolume(Vector3.Zero, radius, 10));
        }

        public static BasicBody Cuboid(double x, double y, double z, Vector3 pos)
        {
            var wall = new BasicBody(
                new RigidBody6DOF(
                    new EuclideanKinematics(new Transform(pos, Matrix3.Identity), Vector3.Zero, Vector3.Zero),
                    1.0,
                    Matrix3.Identity),
                None.Instance,
                new BasicMaterial(),
                Intersectors.CuboidIntersector(x, y, z),
                AlwaysOverlap.Instance,
                new Cuboid(x, y, z));

            wall.Dynamics.Fix();
            return wall;
        }

        public static IEnumerable<BasicBody> Box(double x, double y, double z, Vector3 center)
        {
            yield return Cuboid(x, y, 1.0, new Vector3(0, 0, -z / 2)); // bottom
            yield return Cuboid(x, y, 1.0, new Vector3(0, 0, z / 2)); // top

            yield return Cuboid(x, 1.0, z, new Vector3(0, -y / 2, 0)); // front
            yield return Cuboid(x, 1.0, z, new Vector3(0, y / 2, 0)); // back

            yield return Cuboid(1.0, y, z, new Vector3(-x / 2, 0, 0)); // left
            yield return Cuboid(1.0, y, z, new Vector3(x / 2, 0, 0)); // right

        }


    }
}
