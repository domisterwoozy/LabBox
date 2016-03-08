using System.Collections.Generic;
using Math3D;
using Math3D.VectorCalc;
using Physics3D.Dynamics;
using Util;
using System.Linq;
using Physics3D.Forces;
using Physics3D.Bodies;
using Physics3D.Collisions;
using Physics3D.Measurement;
using System;

namespace Physics3D.Universes
{
    public class BasicUniverse : IUniverse
    {
        public event EventHandler<FrameLengthArgs> FrameFinished;

        public double UniversalTime { get; private set; }

        public ICollection<IBody> Bodies { get; } = new List<IBody>();

        public IContactResolver ContactResolver { get; set; } = NoCollisions.Instance;
        public IContactFinder ContactFinder { get; set; } = NoContacts.Instance;

        public ICollection<ForceField> ForceFields { get; } = new List<ForceField>();              

        public void Update(double deltaTime)
        {
            if (deltaTime == 0) return;
            //deltaTime = Math.Pow(10, -5); // constant for testing
            foreach (IBody body in Bodies)
            {
                if (body.Dynamics.IsFixed) continue;

                // enact forces on the body
                foreach (ForceField field in ForceFields)
                {
                    body.Dynamics.ThrustSingleFrame(field.GetForceOnBody(body), field.GetTorqueOnBody(body));
                }
                // update body state
                body.Dynamics.Update(deltaTime);

                // find and resolve contacts
                ContactResolver.ResolveContacts(ContactFinder.FindContacts(body, Bodies));
            }

            UniversalTime += deltaTime;
            FrameFinished?.Invoke(this, new FrameLengthArgs(deltaTime));
        }
    }
}
