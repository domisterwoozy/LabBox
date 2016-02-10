using Math3D.VectorCalc;
using Physics3D.Dynamics;
using System.Collections.Generic;

namespace Physics3D.Universes
{
    public interface IUniverse
    {
        ICollection<IDynamicBody> DynamicBodies { get; }
        ICollection<IScalarField> Potentials { get; }
        ICollection<IVectorField> ForceFields { get; }

        void Update(double deltaTime);
    }
}
