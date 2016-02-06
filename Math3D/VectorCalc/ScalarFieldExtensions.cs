using Math3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.VectorCalc
{
    public static class ScalarFieldExtensions
    {
        public static IScalarField Within(this IScalarField f, double radOfInf) => new ClampedScalarField(f, pos => pos.MagSquared <= radOfInf * radOfInf);
        public static IScalarField Add(this IScalarField f, IScalarField other) => new SumScalarField(f, other);
        public static IScalarField Translate(this IScalarField f, Vector3 translation) => new TranslatedScalarField(f, translation);      
    }
}
