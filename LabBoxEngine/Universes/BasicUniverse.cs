using Physics3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Interfaces;
using Math3D;

namespace Physics3D
{
    public class BasicUniverse : IUniverse
    {
        public ICollection<IDynamicBody> DynamicBodies { get; } = new List<IDynamicBody>();
        public ICollection<IVectorField> ForceFields { get; } = new List<IVectorField>();
        public ICollection<IScalarField> Potentials { get; } = new List<IScalarField>();

        public void Update(double deltaTime)
        {
            // enact all the single frame forces on the bodies
            foreach (IDynamicBody body in DynamicBodies)
            {
                foreach (IVectorField field in ForceFields)
                {
                    body.ThrustSingleFrame(field.Value(body.Kinematics.Transform.Pos),
                        Vector3.Zero); // possible non conservative torque component of vector fields are ignored
                }
                foreach (IScalarField potential in Potentials)
                {
                    body.ThrustSingleFrame(potential.ToVectorField().Value(body.Kinematics.Transform.Pos), Vector3.Zero);
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
