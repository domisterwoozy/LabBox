using Math3D.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Math3D
{
    /// <summary>
    /// Represents a position and orientation in 3D space. Can be thought of as a cartesian coordinate system.
    /// Todo: performance analysis to see if we should make this an immutable class.
    /// </summary>
    public struct Transform
    {
        public static readonly Transform Zero = new Transform(Vector3.Zero, Matrix3.Identity);
        public static readonly Transform World = Zero;

        /// <summary>
        /// The world position of the center of mass of this body.
        /// </summary>
        public Vector3 Pos { get;  }        

        /// <summary>
        /// The orientation of this body in world coordinates.
        /// </summary>
        public Quaternion Q { get; }

        /// <summary>
        /// The orientation of the body in world coordinates.
        /// </summary>
        public Matrix3 R => Q.ToMatrix();

        /// <summary>
        /// The 'i' basis vector of this transform's local coordinates in world coordinates.
        /// </summary>
        public Vector3 I => ToWorldSpace(Vector3.I);

        /// <summary>
        /// The 'j' basis vector of this transform's local coordinates in world coordinates.
        /// </summary>
        public Vector3 J => ToWorldSpace(Vector3.J);

        /// <summary>
        /// The 'k' basis vector of this transform's local coordinates in world coordinates.
        /// </summary>
        public Vector3 K => ToWorldSpace(Vector3.K);

        public Transform(Vector3 pos, Quaternion q)
        {
            if (!q.IsUnit) throw new ArgumentException(nameof(q) + "must be a unit quaternion");
            Pos = pos;
            Q = q;
        }

        public Transform(Vector3 pos, Matrix3 r)
        {
            Pos = pos;
            Q = Quaternion.FromRotMatrix(r);
        }   
        
        /// <summary>
        /// Gets the unit vector directions in world space of this local coordinate spaces basis vectors.
        /// </summary>
        public Vector3 GetBasis(Axis axes)
        {
            switch (axes)
            {
                case Axis.X:
                    return I;
                case Axis.Y:
                    return J;
                case Axis.Z:
                    return K;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axes));
            }
        }

        public Transform Translate(Vector3 v) => new Transform(Pos + v, Q);

        public Transform Rotate(Matrix3 m) => Rotate(Quaternion.FromRotMatrix(m));

        public Transform Rotate(Quaternion q)
        {
            Debug.Assert(q.IsUnit);
            return new Transform(Pos, q * Q);
        }

        #region Transformations
        /// <summary>
        /// Converts a point in this local transforms coordinates to world coordinates.
        /// </summary>
        public Vector3 ToWorldSpace(Vector3 localPoint) => R * localPoint + Pos;

        public Vector3 RotateToWorld(Vector3 localDir) => R * localDir;

        public Edge ToWorldSpace(Edge localEdge) => new Edge(ToWorldSpace(localEdge.A), ToWorldSpace(localEdge.B));

        public Intersection ToWorldSpace(Intersection localInter) => new Intersection(ToWorldSpace(localInter.Point), RotateToWorld(localInter.Normal));

        /// <summary>
        /// Converts a point in world space to this local transform's coordinate space.
        /// </summary>
        public Vector3 ToLocalSpace(Vector3 worldPoint) => R.TransposeMatrix() * (worldPoint - Pos);

        public Vector3 RotateToLocal(Vector3 worldDir) => R.TransposeMatrix() * worldDir;

        public Edge ToLocalSpace(Edge worldEdge) => new Edge(ToLocalSpace(worldEdge.A), ToLocalSpace(worldEdge.B));

        public Intersection ToLocalSpace(Intersection worldInter) => new Intersection(ToLocalSpace(worldInter.Point), RotateToLocal(worldInter.Normal));
        #endregion



        #region Equality
        /// <summary>
        /// Transform equality.
        /// </summary>
        public static bool operator ==(Transform a, Transform b) => a.Equals(b);

        /// <summary>
        /// Transform inequality.
        /// </summary>
        public static bool operator !=(Transform a, Transform b) => !(a == b);

        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + Pos.GetHashCode();
            result = 31 * result + Q.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Transform t;
            if (obj is Transform)
            {
                t = (Transform)obj;
            }
            else
            {
                return false;
            }

            return Equals(t);
        }

        public bool Equals(Transform other)
        {
            if (Pos != other.Pos) return false;
            return Q == other.Q;
        }
        #endregion
    }

    public struct Transformation
    {
        public Transform TransformA { get; }
        public Transform TransformB { get; }

        public Transformation(Transform a, Transform b)
        {
            TransformA = a;
            TransformB = b;
        }

        public Transformation Reverse() => new Transformation(TransformB, TransformA);

        public Vector3 TransformPos(Vector3 v) => TransformB.ToLocalSpace(TransformA.ToWorldSpace(v));
        public Vector3 TransformDirection(Vector3 v) => TransformB.RotateToLocal(TransformA.RotateToWorld(v));
        public IEnumerable<Vector3> TransformDirections(params Vector3[] vectors) => vectors.Select(TransformDirection);
        public IEnumerable<Vector3> TransformPositions(params Vector3[] vectors) => vectors.Select(TransformPos);


        public Edge TransformEdge(Edge e) => new Edge(TransformPos(e.A), TransformPos(e.B));
        public Intersection TransformIntersection(Intersection i) => new Intersection(TransformPos(i.Point), TransformDirection(i.Normal));
    }
}
