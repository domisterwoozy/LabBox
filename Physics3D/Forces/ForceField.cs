using Math3D;
using Math3D.VectorCalc;
using Physics3D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Physics3D.Forces
{
    public delegate Vector3 ForceApplicationFunc(IBody body, Vector3 rawForce);

    /// <summary>
    /// Nearly all forces are not completely independent of the body they are acting on (ex: classical gravity, electromagnetism, drag, etc)
    /// This class allows the ability to apply a basic vector force field to a body while considering properties of the body besides position.
    /// (ex: mass, velocity, charge, etc).
    /// </summary>
    public class ForceField
    {
        public Generator<IVectorField> RawField { get; }
        public ForceApplicationFunc ForceApplicationFunc { get; }

        public ForceField(Generator<IVectorField> rawField, ForceApplicationFunc applFunc)
        {
            RawField = rawField;
            ForceApplicationFunc = applFunc;
        }

        public Vector3 GetForceOnBody(IBody body) => ForceApplicationFunc(body, RawField().Value(body.Dynamics.Transform.Pos)); 
    }

    public static class ForceFields
    {
        public static Vector3 DragApplier(IBody body, Vector3 windField)
        {
            double area = body.Shape.CrossSectionalArea(body.Dynamics.Transform.Pos, body.Dynamics.Kinematics.V);
            return body.Dynamics.Kinematics.V.MagSquared  * area * body.Material.DragCoef * windField;
        }

        public static Vector3 GravityApplier(IBody body, Vector3 gravField) => body.Dynamics.Mass * gravField;

        public static Vector3 ElectricApplier(IBody body, Vector3 electricField) => body.EMProps.Charge * electricField;

        public static Vector3 MagnetismApplier(IBody body, Vector3 magneticField)
        {
            throw new NotImplementedException();
        }


        public static ForceField Drag(IVectorField windField) => new ForceField(() => windField, DragApplier);

        public static ForceField Gravity(IDynamicBody sourceBody, double gravConstant) => Gravity(() => sourceBody.Transform.Pos, sourceBody.Mass * gravConstant);

        public static ForceField Gravity(Func<Vector3> posFunc, double strength)
        {
            return new ForceField(() => FieldExtensions.SphericalField(-strength, -1).Translate(posFunc()).ToVectorField(), GravityApplier);
        }


    }

}
