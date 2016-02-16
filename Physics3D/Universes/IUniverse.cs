using Math3D.VectorCalc;
using Physics3D.Bodies;
using Physics3D.Dynamics;
using Physics3D.Forces;
using System.Collections.Generic;
using Util;

namespace Physics3D.Universes
{
    public interface IUniverse
    {
        ICollection<IBody> Bodies { get; }
        ICollection<ForceField> ForceFields { get; }

        void Update(double deltaTime);
    }
}
