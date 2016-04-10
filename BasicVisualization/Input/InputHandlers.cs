using LabBox.Visualization.Universe.ViewModel;
using Math3D;
using Physics3D.Bodies;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Input
{
    public static class InputHandlers
    {
        public static ItemSelector<IBody> BodySelector(IInputObservable input, IUniverse uni, ICamera cam)
        {
            return BodySelector(input, uni, () => cam.Pos, () => (cam.LookAtPos - cam.Pos).UnitDirection);
        }

        public static ItemSelector<IBody> BodySelector(IInputObservable input, IUniverse uni, Vector3 rayOrigin, Vector3 rayDir)
        {
            return BodySelector(input, uni, () => rayOrigin, () => rayDir);
        }

        public static ItemSelector<IBody> BodySelector(IInputObservable input, IUniverse uni, Func<Vector3> rayOrigin, Func<Vector3> rayDir)
        {
            return new ItemSelector<IBody>(input, () => uni.RaySelect(rayOrigin(), rayDir()));
        }
    }
}
