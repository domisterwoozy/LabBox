using System.Collections.Generic;
using Math3D;
using Math3D.VectorCalc;
using Physics3D.Dynamics;
using Util;
using System.Linq;
using Physics3D.Forces;
using Physics3D.Bodies;

namespace Physics3D.Universes
{
    public class BasicUniverse : IUniverse
    {
        public ICollection<IBody> Bodies { get; } = new List<IBody>();
        public ICollection<ForceField> ForceFields { get; } = new List<ForceField>();

        public void Update(double deltaTime)
        {
            // enact all the single frame forces on the bodies
            foreach (IBody body in Bodies)
            {
                foreach (ForceField field in ForceFields)
                {
                    body.Dynamics.ThrustSingleFrame(field.GetForceOnBody(body), field.GetTorqueOnBody(body));
                }            
            }

            // update all the bodies
            foreach (IBody body in Bodies)
            {
                body.Dynamics.Update(deltaTime);
            }
        }
    }
}
