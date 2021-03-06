﻿using Math3D.Geometry;

namespace Math3D.VectorCalc
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
    }
}
