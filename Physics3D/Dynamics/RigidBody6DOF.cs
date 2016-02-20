using System;
using System.Collections.Generic;
using System.Linq;
using Math3D;
using System.Diagnostics;
using Math3D.Geometry;
using Physics3D.Kinematics;
using Physics3D.Universes;

namespace Physics3D.Dynamics
{
    public class RigidBody6DOF : IDynamicBody
    {
        public event EventHandler<FrameLengthArgs> FrameFinished;

        // a temp force/torque (a thrust)
        private class TempForce
        {
            public Vector3 Force { get; }
            public double DurationRemaining { get; set; }

            public TempForce(Vector3 force, double duration)
            {
                Debug.Assert(duration > 0);
                Force = force;
                DurationRemaining = duration;
            }
        }

        private readonly IKinematicBody kinematicBody;

        // inputs
        private Vector3 netForce = Vector3.Zero;
        private Vector3 netTorque = Vector3.Zero;
        private readonly HashSet<TempForce> tempForces = new HashSet<TempForce>();
        private readonly HashSet<TempForce> tempTorques = new HashSet<TempForce>();
        private readonly List<Vector3> singleFrameForces = new List<Vector3>();
        private readonly List<Vector3> singleFrameTorques = new List<Vector3>();
        private IManifold constraint = null;

        // derived properties
        public IKinematics Kinematics => kinematicBody.GetCurrentState();
        public Transform Transform => kinematicBody.Transform;
        public Matrix3 I => InvI.InverseMatrix();
        public Matrix3 InvI => InvIBody.Rotate(kinematicBody.Transform.R);
        public double Mass => 1 / InvMass;
        public double KineticEnergy
        {
            get
            {
                Vector3 n = kinematicBody.Omega.UnitDirection; // direciton of current angular vel
                double scalarInertia = ScalarInertia(n);
                double rotEnergy = scalarInertia == double.PositiveInfinity ? 0 : 0.5 * scalarInertia * kinematicBody.Omega.Magnitude;
                double transEnergy = IsFixed ? 0 : 0.5 * Mass * kinematicBody.V.Magnitude;
                return rotEnergy + transEnergy;
            }
        }
        public bool IsFixed => IsPositionFixed && IsRotationFixed;
        public bool IsPositionFixed => InvMass == 0;
        public bool IsRotationFixed => InvI == Matrix3.Zero;
        public Vector3 NetCurrentForce
        {
            get
            {
                Vector3 tempSum = Vector3.Zero;
                foreach (Vector3 force in tempForces.Select(tf => tf.Force)) tempSum += force;
                foreach (Vector3 force in singleFrameForces) tempSum += force;
                return netForce + tempSum;
            }

        }
        public Vector3 NetCurrentTorque
        {
            get
            {
                Vector3 tempSum = Vector3.Zero;
                foreach (Vector3 torque in tempTorques.Select(tt => tt.Force)) tempSum += torque;
                foreach (Vector3 torque in singleFrameTorques) tempSum += torque;
                return netTorque + tempSum;
            }
        }

        // updated when major structural changes occur
        public Matrix3 InvIBody { get; private set; }
        public double InvMass { get; private set; }

        // updated every frame
        public Vector3 L { get; private set; }     
        public Vector3 P { get; private set; }        

        public RigidBody6DOF(IKinematics initialState, double mass, Matrix3 inertiaBody)
        {
            if (initialState == null) throw new ArgumentNullException(nameof(initialState));
            if (mass <= 0) throw new ArgumentException(nameof(mass)  + " must be larger than zero");
            InvMass = 1.0 / mass;
            InvIBody = inertiaBody.InverseMatrix();

            kinematicBody = new EuclideanKinematicBody(initialState.Transform);
            kinematicBody.V = initialState.V;
            kinematicBody.Omega = initialState.Omega;

            P = Mass * kinematicBody.V;
            L = I * kinematicBody.Omega;
        }

        /// <summary>
        /// The amount of scalar inertia about a specified axis. 
        /// </summary>
        /// <param name="dir">The direction of the axis in world coordinates</param>
        public double ScalarInertia(Vector3 dir) => IsRotationFixed ? double.PositiveInfinity : dir * (I * dir);

        public bool IsAxisFixed(Axis axis)
        {
            Vector3[] rowVects = InvIBody.RowVectors;
            if (axis == Axis.X) return rowVects[0] == Vector3.Zero;
            if (axis == Axis.Y) return rowVects[1] == Vector3.Zero;
            if (axis == Axis.Z) return rowVects[2] == Vector3.Zero;
            throw new ArgumentOutOfRangeException(nameof(axis));
        }        

        public void ConstraintToManifold(IManifold manifold)
        {
            constraint = manifold;
        }

        public void EnactImpulse(Vector3 impulse, Vector3 relPos)
        {
            P += impulse;
            L += relPos ^ impulse;
        }

        public void Fix()
        {
            FixRotation();
            FixPosition();
        }

        public void FixAxes(ISet<Axis> axes)
        {
            if (axes == null) throw new ArgumentNullException(nameof(axes));
            if (axes.Count == 0) return;

            Vector3[] rowVects = InvIBody.RowVectors;
            if (axes.Contains(Axis.X)) rowVects[0] = Vector3.Zero;            
            if (axes.Contains(Axis.Y)) rowVects[1] = Vector3.Zero;
            if (axes.Contains(Axis.Z)) rowVects[2] = Vector3.Zero;

            InvIBody = Matrix3.FromRowVectors(rowVects[0], rowVects[1], rowVects[2]);
        }

        public void FixPosition()
        {
            InvMass = 0;
        }

        public void FixRotAroundAxis(Vector3 axisBody, double scalarInertia)
        {
            InvIBody = scalarInertia * Matrix3.Identity; // reset to identity
            FixAxes(new HashSet<Axis> { Axis.X, Axis.Y }); // lock everything but z axis rotation
            Quaternion rot = Vector3.K.RotationRequired(axisBody);
            InvIBody = InvIBody.Rotate(rot);
            // no matter what you multiply invI by the result will always be proportional to axisBody
            // this means that no matter what angular momentum you put on the body the angular velocity
            // will always be pointing in the direction of axisBody
            // this effectively locks it to an axis
        }

        public void FixRotation()
        {
            InvIBody = Matrix3.Zero;
        }

        public void AddInputs(Vector3 force, Vector3 torque)
        {
            netForce += force;
            netTorque += torque;
        }

        public void ThrustInputs(Vector3 force, Vector3 torque, float duration)
        {
            tempForces.Add(new TempForce(force, duration));
            tempTorques.Add(new TempForce(torque, duration));
        }

        public void ThrustSingleFrame(Vector3 force, Vector3 torque)
        {
            singleFrameForces.Add(force);
            singleFrameTorques.Add(torque);
        }

        public void Update(double deltaTime)
        { 
            // update forces           
            P += deltaTime * NetCurrentForce;
            L += deltaTime * NetCurrentTorque;
            // update velocities
            kinematicBody.V = InvMass * P;
            kinematicBody.Omega = InvI * L;
            // update positions
            kinematicBody.UpdateTransform(deltaTime);

            UpdateTempForces(deltaTime);
            FrameFinished?.Invoke(this, new FrameLengthArgs(deltaTime));
        }

        private void UpdateTempForces(double deltaTime)
        {
            foreach(TempForce force in tempForces) force.DurationRemaining -= deltaTime;
            tempForces.RemoveWhere(tf => tf.DurationRemaining <= 0);

            foreach (TempForce torque in tempTorques) torque.DurationRemaining -= deltaTime;
            tempTorques.RemoveWhere(tt => tt.DurationRemaining <= 0);

            singleFrameForces.Clear();
            singleFrameTorques.Clear();
        }        
    }
}
