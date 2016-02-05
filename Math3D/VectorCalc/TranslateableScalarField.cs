﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math3D.Interfaces;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A mutable wrapper that allows you to move a scalar field in the world.
    /// </summary>
    public class TranslateableScalarField : AbstractScalarField
    {
        /// <summary>
        /// The underlying scalar field that can be translated.
        /// </summary>
        public IScalarField UnderlyingScalarField { get; }

        /// <summary>
        /// The position in 3D space of the underlying scalar field.
        /// </summary>
        public Vector3 Position => PositionFunc();

        /// <summary>
        /// Generates the current position of the underlying field.
        /// </summary>
        public Func<Vector3> PositionFunc { get; }

        public TranslateableScalarField(IScalarField underlyingScalarField, Func<Vector3> positionFunc)
        {
            UnderlyingScalarField = underlyingScalarField;
            PositionFunc = positionFunc;
        }

        public override double Value(Vector3 pos) => UnderlyingScalarField.Value(pos - Position);

        public override Vector3 Gradient(Vector3 pos) => UnderlyingScalarField.Gradient(pos - Position);

        public override IVectorField ToVectorField() => new TranslateableVectorField(UnderlyingScalarField.ToVectorField()) { Position = Position };

        public override Vector3 GradientTraversal(Vector3 pos, double desiredPotential, double tolerance)
        {
            // use the underlying fields gradient traversal in case it overrode the abstract version
            return UnderlyingScalarField.GradientTraversal(pos - Position, desiredPotential, tolerance);
        }
    }
}
