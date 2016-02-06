using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Interfaces
{
    public interface IScalarField
    {
        /// <summary>
        /// That value of the ScalerField at a certain global position.
        /// </summary>
        double Value(Vector3 pos);

        /// <summary>
        /// The gradient of the scalar field at a specified point.
        /// </summary>
        Vector3 Gradient(Vector3 pos);

        /// <summary>
        /// Generates a vector field corresponding to the negative gradient of this scalar field.
        /// If the scalar field represents a potential then this function will return a force field
        /// representing the force enacted by the potential.
        /// </summary>
        IVectorField ToVectorField();

        /// <summary>
        /// Traverses a path of steepest ascent/descent until the desired potential is reached.
        /// </summary>
        /// <param name="pos">Starting point</param>
        /// <param name="desiredPotential">The field value you want to reach</param>
        /// <returns>The position the traversal ends at</returns>
        Vector3 GradientTraversal(Vector3 pos, double desiredPotential, double tolerance);

        /// <summary>
        /// Generates a manifold from this scalar field at a certain potential.
        /// This is an equipotential surface of this scalar field.
        /// </summary>
        IManifold ToManifold(double potential);
    }
}
