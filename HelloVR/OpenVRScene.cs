using OpenTK; // only using OpenTK temperarily so I can use there Matrix and Vector objects
using System;
using System.Linq;
using Util;
using Valve.VR;

namespace HelloVR
{
    public class OpenVRScene
    {
        // hmd readonly fields  
        private readonly CVRSystem hmd;

        private readonly Matrix4 leftEyeProj;
        private readonly Matrix4 rightEyeProj;
        private readonly Matrix4 leftEyeView;
        private readonly Matrix4 rightEyeView;

        private readonly int leftHandControllerIndex;
        private readonly int rightHandControllerIndex;

        private IVRGraphics graphics;           

        // hmd volitile fields
        private TrackedDevicePose_t[] devicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        private Matrix4 hmdViewMatrix;

        // properties
        public float NearClip { get; }
        public float FarClip { get; }
        public OpenVRInputObservable OpenVRInput { get; }

        public VRControllerAxis_t LeftTouchpadAxis => hmd.ControllerState(leftHandControllerIndex).rAxis0;
        public VRControllerAxis_t RightTouchpadAxis => hmd.ControllerState(rightHandControllerIndex).rAxis0;

        public Result<string, ETrackedPropertyError> DeviceInfo(int deviceIndex, ETrackedDeviceProperty prop)
            => hmd.TrackedDeviceString(deviceIndex, ETrackedDeviceProperty.Prop_RenderModelName_String);
        public bool IsConnected(int deviceIndex) => hmd.IsTrackedDeviceConnected((uint)deviceIndex);
        public bool ValidPose(int deviceIndex) => devicePoses[deviceIndex].bPoseIsValid;

        public Matrix4 TrackedPose(int deviceIndex) => devicePoses[deviceIndex].mDeviceToAbsoluteTracking.ToGLMatrix4();
        public Vector3 TrackedPosition(int deviceIndex) => TrackedPose(deviceIndex).ExtractTranslation();
        public Quaternion TrackedRot(int deviceIndex) => TrackedPose(deviceIndex).ExtractRotation();

        public Vector3 HMDPosition => TrackedPosition((int)OpenVR.k_unTrackedDeviceIndex_Hmd);
        public Vector3 LeftEyePos => HMDPosition + leftEyeView.ExtractTranslation(); // not sure if these are right
        public Vector3 RightEyePos => HMDPosition + leftEyeView.ExtractTranslation(); // not sure if these are right
        public Vector3 LeftControllerPos => TrackedPosition(leftHandControllerIndex);
        public Vector3 RightControllerPos => TrackedPosition(rightHandControllerIndex);

        private OpenVRScene(float nearClip, float farClip, CVRSystem hmd, int leftIndex, int rightIndex)
        {
            // set up the hmd
            this.hmd = hmd;

            NearClip = nearClip;
            FarClip = farClip;

            leftEyeProj = hmd.GetProjectionMatrix(EVREye.Eye_Left, nearClip, farClip, EGraphicsAPIConvention.API_OpenGL).ToGLMatrix4();
            rightEyeProj = hmd.GetProjectionMatrix(EVREye.Eye_Right, nearClip, farClip, EGraphicsAPIConvention.API_OpenGL).ToGLMatrix4();
            leftEyeView = hmd.GetEyeToHeadTransform(EVREye.Eye_Left).ToGLMatrix4().Inverted();
            rightEyeView = hmd.GetEyeToHeadTransform(EVREye.Eye_Right).ToGLMatrix4().Inverted();
            leftHandControllerIndex = leftIndex;
            rightHandControllerIndex = rightIndex;

            OpenVRInput = new OpenVRInputObservable(hmd);
        }

        public static OpenVRScene Create(float nearClip, float farClip)
        {
            EVRInitError error = EVRInitError.None;
            CVRSystem hmd = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Scene);
            if (error != EVRInitError.None) throw new InvalidOperationException($"Unable to initilize OpenVR: {error}");

            string[] failedProps = OpenVRUtil.GetFailedRequiredComponents().ToArray();
            if (failedProps.Length != 0) throw new InvalidOperationException($"Failed to initialize the following static properties: {string.Join(", ", failedProps)}");

            int leftIndex = hmd.LeftControllerIndex().Validate(errMsg => new InvalidOperationException(errMsg));
            int rightIndex = hmd.RightControllerIndex().Validate(errMsg => new InvalidOperationException(errMsg));

            return new OpenVRScene(nearClip, farClip, hmd, leftIndex, rightIndex);
        }

        public void InitGraphics(IVRGraphics vrGraphics)
        {
            graphics = vrGraphics;
        }

        public Tuple<int, int> GetRenderTarget()
        {
            uint uRenderWidth = 0;
            uint uRenderHeight = 0;
            hmd.GetRecommendedRenderTargetSize(ref uRenderWidth, ref uRenderHeight);
            return Tuple.Create((int)uRenderWidth, (int)uRenderHeight);
        }

        public void UpdateTracking()
        {
            var err = OpenVR.Compositor.WaitGetPoses(devicePoses, new TrackedDevicePose_t[0]);
            if (err != EVRCompositorError.None) throw new InvalidOperationException($"Failed to get poses: {err}");
            TrackedDevicePose_t hmdPose = devicePoses[OpenVR.k_unTrackedDeviceIndex_Hmd];
            if (!hmdPose.bPoseIsValid) throw new InvalidOperationException("HMD pose is not valid");
            hmdViewMatrix = hmdPose.mDeviceToAbsoluteTracking.ToGLMatrix4().Inverted();
        }

        public void RenderFrame(params IVRDrawable[] drawables)
        {
            if (graphics == null) throw new InvalidOperationException("Cannot render a frame until graphics are initialized");

            EyeTextures eyeTextures = graphics.RenderToTextures(
                CurrentViewProjMatrix(EVREye.Eye_Left),
                CurrentViewProjMatrix(EVREye.Eye_Right),
                LeftEyePos,
                RightEyePos,
                drawables);

            Texture_t leftEye = eyeTextures.LeftEye;
            Texture_t rightEye = eyeTextures.RightEye;

            // send to the headset
            VRTextureBounds_t bounds = new VRTextureBounds_t() { uMin = 0, vMin = 0, uMax = 1f, vMax = 1f }; // is this right?
            EVRCompositorError compErr = OpenVR.Compositor.Submit(EVREye.Eye_Left, ref leftEye, ref bounds, EVRSubmitFlags.Submit_Default);
            if (compErr != EVRCompositorError.None) throw new InvalidOperationException($"Failed to submit image to compositor: {compErr}");
            compErr = OpenVR.Compositor.Submit(EVREye.Eye_Right, ref rightEye, ref bounds, EVRSubmitFlags.Submit_Default);
            if (compErr != EVRCompositorError.None) throw new InvalidOperationException($"Failed to submit image to compositor: {compErr}");
        }

        private Matrix4 CurrentViewProjMatrix(EVREye eye)
        {
            switch (eye)
            {
                case EVREye.Eye_Left:
                    return hmdViewMatrix * leftEyeView * leftEyeProj;
                case EVREye.Eye_Right:
                    return hmdViewMatrix * rightEyeView * rightEyeProj;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eye));
            }
        }
    }
}
