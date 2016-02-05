using Math3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Interfaces
{
    public interface IUniverse
    {
        ICollection<IDynamicBody> DynamicBodies { get; }
        ICollection<IScalarField> Potentials { get; }
        ICollection<IVectorField> ForceFields { get; }

        void Update(double deltaTime);
    }
}
