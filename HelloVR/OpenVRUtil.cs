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

namespace HelloVR
{
    public static class OpenVRUtil
    {
        //public static Matrix4 ToGLMatrix4(this HmdMatrix34_t matPose)
        //{
        //    return new Matrix4(
        //        matPose.m0, matPose.m1, matPose.m2, matPose.m3, // might need to be column first?
        //        matPose.m4, matPose.m5, matPose.m6, matPose.m7,
        //        matPose.m8, matPose.m9, matPose.m10, matPose.m11,
        //        0.0f, 0.0f, 0.0f, 1.0f
        //    );
        //}

        //public static Matrix4 ToGLMatrix4(this HmdMatrix44_t matPose)
        //{
        //    return new Matrix4(
        //        matPose.m0, matPose.m1, matPose.m2, matPose.m3, // might need to be column first?
        //        matPose.m4, matPose.m5, matPose.m6, matPose.m7,
        //        matPose.m8, matPose.m9, matPose.m10, matPose.m11,
        //        matPose.m12, matPose.m13, matPose.m14, matPose.m15
        //    );
        //}

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

        /// <summary>
        /// Helper to get a string from a tracked device property
        /// </summary>
        public static string GetTrackedDeviceStr(this CVRSystem hmd, uint trackedDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError error)
        {
            uint requiredBufferLen = hmd.GetStringTrackedDeviceProperty(trackedDeviceIndex, prop, null, 0, ref error);
            if (requiredBufferLen == 0) return "";

            var pchBuffer = new StringBuilder((int)requiredBufferLen);
            requiredBufferLen = hmd.GetStringTrackedDeviceProperty(trackedDeviceIndex, prop, pchBuffer, requiredBufferLen, ref error);
            return pchBuffer.ToString();
        }

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

        


    }    

    

}
