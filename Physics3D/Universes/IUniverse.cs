using Math3D.VectorCalc;
using Physics3D.Dynamics;
using Physics3D.Forces;
using System.Collections.Generic;
using Util;

namespace Physics3D.Universes
{
    public interface IUniverse
    {
        ICollection<IBody> DynamicBodies { get; }
        ICollection<ForceField> ForceFields { get; }
        ICollection<Generator<IVectorField>> BasicForces { get; }

        void Update(double deltaTime);
    }
}
