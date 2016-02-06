using Math3D;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTests
{
    /// <summary>
    /// Entry point to use the custom constraints. Wouldn't it be nice if we could implement static extension methods?
    /// For now we will use 'Iz'.
    /// </summary>
    public class Iz : Is
    {
        public static VectorEqualConstraint EqualTo(Vector3 expected)
        {
            return new VectorEqualConstraint(expected);
        }
    }

    public class VectorEqualConstraint : Constraint
    {
        private readonly Vector3 expected;
        private readonly Vector3 compTolerances;

        public VectorEqualConstraint(Vector3 expected) : this(expected, Vector3.Zero)
        {
        }

        public VectorEqualConstraint(Vector3 expected, Vector3 compTolerances)
        {
            this.expected = expected;
            this.compTolerances = compTolerances;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Vector3? vNullable = actual as Vector3?;
            if (vNullable == null)
            {
                return new ConstraintResult(this, actual, ConstraintStatus.Failure);
            }

            Vector3 v = vNullable.Value;
            if (Math.Abs(v.X - expected.X) > compTolerances.X) return new ConstraintResult(this, actual, ConstraintStatus.Failure);
            if (Math.Abs(v.Y - expected.Y) > compTolerances.Y) return new ConstraintResult(this, actual, ConstraintStatus.Failure);
            if (Math.Abs(v.Z - expected.Z) > compTolerances.Z) return new ConstraintResult(this, actual, ConstraintStatus.Failure);
            return new ConstraintResult(this, actual, ConstraintStatus.Success);
        }

        public VectorEqualConstraint Within(double compTolerance)
        {
            if (compTolerances != Vector3.Zero)
            {
                throw new InvalidOperationException("Within modifier may appear only once in a constraint expression");
            }
            return new VectorEqualConstraint(expected, new Vector3(compTolerance, compTolerance, compTolerance));
        }

        public VectorEqualConstraint Within(Vector3 compTolerances)
        {
            if (compTolerances != Vector3.Zero)
            {
                throw new InvalidOperationException("Within modifier may appear only once in a constraint expression");
            }
            return new VectorEqualConstraint(expected, compTolerances);
        }
    }
}
