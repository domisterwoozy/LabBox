using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Threading;
using Util;

namespace HelloVR
{
    public static class OpenVRUtil
    {
        public static IEnumerable<string> GetFailedRequiredComponents()
        {
            if (OpenVR.System == null) yield return nameof(OpenVR.System);
            if (OpenVR.Chaperone == null) yield return nameof(OpenVR.Chaperone);
            if (OpenVR.ChaperoneSetup == null) yield return nameof(OpenVR.ChaperoneSetup);
            if (OpenVR.Compositor == null) yield return nameof(OpenVR.Compositor);
            if (OpenVR.Overlay == null) yield return nameof(OpenVR.Overlay);
            if (OpenVR.RenderModels == null) yield return nameof(OpenVR.RenderModels);
            if (OpenVR.Applications == null) yield return nameof(OpenVR.Applications);
            if (OpenVR.Settings == null) yield return nameof(OpenVR.Settings);
            //if (OpenVR.ExtendedDisplay == null) yield return nameof(OpenVR.ExtendedDisplay); // dont know what this is
        }

        public static Matrix4 ToGLMatrix4(this HmdMatrix34_t matPose)
        {
            return new Matrix4(
                matPose.m0, matPose.m4, matPose.m8, 0.0f,
                matPose.m1, matPose.m5, matPose.m9, 0.0f,
                matPose.m2, matPose.m6, matPose.m10, 0.0f,
                matPose.m3, matPose.m7, matPose.m11, 1.0f
            );
        }

        public static Matrix4 ToGLMatrix4(this HmdMatrix44_t matPose)
        {
            return new Matrix4(
                matPose.m0, matPose.m4, matPose.m8, matPose.m12,
                matPose.m1, matPose.m5, matPose.m9, matPose.m13,
                matPose.m2, matPose.m6, matPose.m10, matPose.m14,
                matPose.m3, matPose.m7, matPose.m11, matPose.m15
            );
        }

        public static IEnumerable<int> DeviceIndexes => Enumerable.Range((int)OpenVR.k_unTrackedDeviceIndex_Hmd, (int)OpenVR.k_unMaxTrackedDeviceCount);

        /// <summary>
        /// Helper to get a string from a tracked device property
        /// </summary>
        public static Result<string, ETrackedPropertyError> TrackedDeviceString(this CVRSystem hmd, int trackedDeviceIndex, ETrackedDeviceProperty prop)
        {
            if (hmd == null) throw new ArgumentNullException(nameof(hmd));
            if (trackedDeviceIndex < 0 || trackedDeviceIndex > OpenVR.k_unMaxTrackedDeviceCount) throw new ArgumentOutOfRangeException(nameof(trackedDeviceIndex));

            ETrackedPropertyError err = ETrackedPropertyError.TrackedProp_Success;
            uint requiredBufferLen = hmd.GetStringTrackedDeviceProperty((uint)trackedDeviceIndex, prop, null, 0, ref err);
            if (err != ETrackedPropertyError.TrackedProp_BufferTooSmall) return Result<string, ETrackedPropertyError>.Err(err);
            if (requiredBufferLen == 0) return "";

            var pchBuffer = new StringBuilder((int)requiredBufferLen);
            requiredBufferLen = hmd.GetStringTrackedDeviceProperty((uint)trackedDeviceIndex, prop, pchBuffer, requiredBufferLen, ref err);
            if (err != ETrackedPropertyError.TrackedProp_Success) return Result<string, ETrackedPropertyError>.Err(err);
            return pchBuffer.ToString();
        }

        public static IEnumerable<string> TrackedDeviceStrings(this CVRSystem hmd, ETrackedDeviceProperty prop)
        {
            if (hmd == null) throw new ArgumentNullException(nameof(hmd));
            return DeviceIndexes.Select(i => hmd.TrackedDeviceString(i, prop)).WhereSuccess();
        }

        public static IEnumerable<int> DevicesInClass(this CVRSystem hmd, ETrackedDeviceClass devClass)
        {
            if (hmd == null) throw new ArgumentNullException(nameof(hmd));
            return DeviceIndexes.Where(i => hmd.GetTrackedDeviceClass((uint)i) == devClass);
        }

        public static Result<int, string> LeftControllerIndex(this CVRSystem hmd)
        {
            if (hmd == null) throw new ArgumentNullException(nameof(hmd));
            int? leftContIndex = 
                hmd.DevicesInClass(ETrackedDeviceClass.Controller)
                .SingleOrDefault(i => hmd.GetControllerRoleForTrackedDeviceIndex((uint)i) == ETrackedControllerRole.LeftHand);

            return leftContIndex == null ? Result<int, string>.Err("No left controller") : leftContIndex.Value;
        }

        public static Result<int, string> RightControllerIndex(this CVRSystem hmd)
        {
            if (hmd == null) throw new ArgumentNullException(nameof(hmd));
            int? rightContIndex =
                hmd.DevicesInClass(ETrackedDeviceClass.Controller)
                .SingleOrDefault(i => hmd.GetControllerRoleForTrackedDeviceIndex((uint)i) == ETrackedControllerRole.RightHand);

            return rightContIndex == null ? Result<int, string>.Err("No right controller") : rightContIndex.Value;
        }

        public static VRControllerState_t ControllerState(this CVRSystem hmd, int contIndex)
        {
            if (hmd == null) throw new ArgumentNullException(nameof(hmd));
            if (contIndex < 0 || contIndex > OpenVR.k_unMaxTrackedDeviceCount) throw new ArgumentOutOfRangeException(nameof(contIndex));

            VRControllerState_t state = new VRControllerState_t();
            bool success = hmd.GetControllerState((uint)contIndex, ref state);
            if (!success) throw new InvalidOperationException("Unable to retrieve controller state");
            return state;
        }
    }    

    

}
