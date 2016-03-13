using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Math3D.Geometry
{
    public class SphereSphereOverlap : Strategy<IOverlapable, Transformation, bool>
    {
        public SphereSphereOverlap() : base(typeof(SphereBound), typeof(SphereBound)) { }

        protected override bool EnactStrategyInternal(IOverlapable first, IOverlapable second, Transformation inputData)
        {
            var firstSphere = (SphereBound)first;
            var secondSphere = (SphereBound)second;

            Vector3 firstPos = inputData.TransformA.Pos;
            Vector3 secondPos = inputData.TransformB.Pos;

            return (secondPos - firstPos).Magnitude < (firstSphere.Radius + secondSphere.Radius);
        }
    }

    //public class SphereBoxOverlap : Strategy<IOverlapable, Transformation, bool>
    //{
    //    public SphereBoxOverlap() : base(typeof(SphereBound), typeof(BoxBound)) { }

    //    protected override bool EnactStrategyInternal(IOverlapable first, IOverlapable second, Transformation inputData)
    //    {
    //        var sphere = first as SphereBound ?? second as SphereBound;
    //        var box = first as BoxBound ?? second as BoxBound;
    //        if (sphere == null || box == null) throw new ArgumentException("Invalid overlap types for this strategy");

    //        Vector3 spherePos = sphere == first ? inputData.TransformA.Pos : inputData.TransformB.Pos;
    //        Vector3 boxPos = box == first ? inputData.TransformA.Pos : inputData.TransformB.Pos;
    //        Quaternion boxOrientation = box == first ? inputData.TransformA.Q : inputData.TransformB.Q;

    //        return CheckOverlap(sphere, box, spherePos, boxPos, boxOrientation);            
    //    }

    //    private static bool CheckOverlap(SphereBound sphere, BoxBound box, Vector3 spherePos, Vector3 boxPos, Quaternion boxOrientation)
    //    {

    //    }
    //}
}
