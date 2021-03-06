﻿using Util;

namespace Math3D.VectorCalc
{
    /// <summary>
    /// A wrapper that allows you to move a vector field in 3D space.
    /// </summary>
    public class TranslatedVectorField : IVectorField
    {
        private readonly Generator<Vector3> translationGen;

        /// <summary>
        /// The underlying vector field to move around.
        /// </summary>
        public IVectorField UnderlyingVectorField { get; }

        /// <summary>
        /// The position in 3D space of the underlying vector field.
        /// </summary>
        public Vector3 Position => translationGen();

        public TranslatedVectorField(IVectorField underlyingVectField, Generator<Vector3> translationGen)
        {
            UnderlyingVectorField = underlyingVectField;
            this.translationGen = translationGen;
        }

        //public Vector3 Curl(Vector3 pos) => UnderlyingVectorField.Curl(pos - Position);

        //public double Divergence(Vector3 pos) => UnderlyingVectorField.Divergence(pos - Position);

        public Vector3 Value(Vector3 pos) => UnderlyingVectorField.Value(pos - Position);
    }
}
