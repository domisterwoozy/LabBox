using System.Collections.Generic;
using Math3D;
using Math3D.VectorCalc;
using Physics3D.Dynamics;
using Util;
using System.Linq;
using Physics3D.Forces;

namespace Physics3D.Universes
{
    public class BasicUniverse : IUniverse
    {
        public ICollection<IBody> DynamicBodies { get; } = new List<IBody>();
        public ICollection<ForceField> ForceFields { get; } = new List<ForceField>();
        public ICollection<Generator<IVectorField>> BasicForces { get; } = new List<Generator<IVectorField>>();

        public void Update(double deltaTime)
        {
            // enact all the single frame forces on the bodies
            foreach (IBody body in DynamicBodies)
            {
                foreach (ForceField field in ForceFields)
                {
                    body.Dynamics.ThrustSingleFrame(field.GetForceOnBody(body),
                        Vector3.Zero); // possible non conservative torque component of vector fields are ignored
                }
                foreach (IVectorField force in BasicForces.Select(fb => fb()))
                {
                    body.Dynamics.ThrustSingleFrame(force.Value(body.Dynamics.Transform.Pos), Vector3.Zero);
                }                
            }

            // update all the bodies
            foreach (IDynamicBody body in DynamicBodies)
            {
                body.Update(deltaTime);
            }
        }
    }
}
