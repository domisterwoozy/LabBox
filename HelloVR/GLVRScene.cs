using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;
using HelloVR.Shaders;
using System.Drawing;

namespace HelloVR
{
    public class GLVRScene
    {
        private readonly float nearClip = 0.1f;
        private readonly float farClip = 30.0f;

        // hmd readonly fields
        private readonly CVRSystem hmd;    
        private readonly Matrix4 leftEyeProj;
        private readonly Matrix4 rightEyeProj;
        private readonly Matrix4 leftEyeView;
        private readonly Matrix4 rightEyeView;

        // opengl readonly fields
        private readonly int renderWidth;
        private readonly int renderHeight;
        private readonly EyeFrameBuffer leftEyeFrameBuffer;
        private readonly EyeFrameBuffer rightEyeFrameBuffer;
        private readonly GLRenderModels renderModels;

        // hmd volitile fields
        private Matrix4 hmdViewMatrix;

        public Vector3 HMDPos { get; private set; }
        //public Vector3 LeftEyePos { get; private set; }
        //public Vector3 RightEyePos { get; private set; }

        public bool ShowRenderModels { get; set; } = true;

        private GLVRScene(float nearClip, float farClip, CVRSystem hmd, bool showRenderModels)
        {
            // set up the hmd
            this.nearClip = nearClip;
            this.farClip = farClip;
            this.hmd = hmd;

            ShowRenderModels = showRenderModels;
            leftEyeProj = hmd.GetProjectionMatrix(EVREye.Eye_Left, nearClip, farClip, EGraphicsAPIConvention.API_OpenGL).ToGLMatrix4();
            rightEyeProj = hmd.GetProjectionMatrix(EVREye.Eye_Right, nearClip, farClip, EGraphicsAPIConvention.API_OpenGL).ToGLMatrix4();
            leftEyeView = hmd.GetEyeToHeadTransform(EVREye.Eye_Left).ToGLMatrix4().Inverted();
            rightEyeView = hmd.GetEyeToHeadTransform(EVREye.Eye_Right).ToGLMatrix4().Inverted();

            // set up opengl elements
            uint uRenderWidth = 0;
            uint uRenderHeight = 0;
            hmd.GetRecommendedRenderTargetSize(ref uRenderWidth, ref uRenderHeight);
            renderWidth = (int)uRenderWidth;
            renderHeight = (int)uRenderHeight;
            leftEyeFrameBuffer = EyeFrameBuffer.Create(renderWidth, renderHeight);
            rightEyeFrameBuffer = EyeFrameBuffer.Create(renderWidth, renderHeight);
            if (showRenderModels) renderModels = new GLRenderModels(hmd);
        }

        public static GLVRScene InitScene(float nearClip, float farClip, bool showRenderModels = true)
        {
            EVRInitError error = EVRInitError.None;
            CVRSystem hmd = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Scene);
            if (error != EVRInitError.None) throw new InvalidOperationException($"Unable to initilize OpenVR: {error}");            

            string[] failedProps = OpenVRUtil.GetFailedRequiredComponents().ToArray();
            if (failedProps.Length != 0) throw new InvalidOperationException($"Failed to initialize the following static properties: {string.Join(", ", failedProps)}");           
            return new GLVRScene(nearClip, farClip, hmd, showRenderModels);
        }

        public void UpdateTracking()
        {
            var devicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
            var err = OpenVR.Compositor.WaitGetPoses(devicePoses, new TrackedDevicePose_t[0]);
            if (err != EVRCompositorError.None) throw new InvalidOperationException($"Failed to get poses: {err}");
            TrackedDevicePose_t hmdPose = devicePoses[OpenVR.k_unTrackedDeviceIndex_Hmd];
            if (!hmdPose.bPoseIsValid) throw new InvalidOperationException("HMD pose is not valid");
            hmdViewMatrix = hmdPose.mDeviceToAbsoluteTracking.ToGLMatrix4().Inverted();
            HMDPos = hmdPose.mDeviceToAbsoluteTracking.ToGLMatrix4().ExtractTranslation();
            if (ShowRenderModels) renderModels.UpdateTracking(devicePoses);
        }

        public void RenderFrame(params IGLDrawable[] drawables)
        {            
            // left eye
            GL.Enable(EnableCap.Multisample);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, leftEyeFrameBuffer.RenderFrameBufferID);
            GL.Viewport(0, 0, renderWidth, renderHeight);
            RenderEye(EVREye.Eye_Left, drawables);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.Multisample);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, leftEyeFrameBuffer.RenderFrameBufferID);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, leftEyeFrameBuffer.ResolveFrameBufferID);
            GL.BlitFramebuffer(0, 0, renderWidth, renderHeight, 0, 0, renderWidth, renderHeight, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            // right eye
            GL.Enable(EnableCap.Multisample);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, rightEyeFrameBuffer.RenderFrameBufferID);
            GL.Viewport(0, 0, renderWidth, renderHeight);
            RenderEye(EVREye.Eye_Right, drawables);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.Multisample);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, rightEyeFrameBuffer.RenderFrameBufferID);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, rightEyeFrameBuffer.ResolveFrameBufferID);
            GL.BlitFramebuffer(0, 0, renderWidth, renderHeight, 0, 0, renderWidth, renderHeight, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            // convert opengl textures to openvr textures  
            Texture_t leftEyeTexture = new Texture_t { handle = new IntPtr(leftEyeFrameBuffer.ResolveFrameBufferID), eType = EGraphicsAPIConvention.API_OpenGL, eColorSpace = EColorSpace.Auto };
            Texture_t rightEyeTexture = new Texture_t { handle = new IntPtr(rightEyeFrameBuffer.ResolveFrameBufferID), eType = EGraphicsAPIConvention.API_OpenGL, eColorSpace = EColorSpace.Auto };

            // send to the headset
            VRTextureBounds_t bounds = new VRTextureBounds_t() { uMin = 0, vMin = 0, uMax = 1f, vMax = 1f }; // is this right?
            EVRCompositorError compErr = OpenVR.Compositor.Submit(EVREye.Eye_Left, ref leftEyeTexture, ref bounds, EVRSubmitFlags.Submit_Default);
            if (compErr != EVRCompositorError.None) throw new InvalidOperationException($"Failed to submit image to compositor: {compErr}");
            compErr = OpenVR.Compositor.Submit(EVREye.Eye_Right, ref rightEyeTexture, ref bounds, EVRSubmitFlags.Submit_Default);
            if (compErr != EVRCompositorError.None) throw new InvalidOperationException($"Failed to submit image to compositor: {compErr}");
        }

        private void RenderEye(EVREye eye, IEnumerable<IGLDrawable> drawables)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (ShowRenderModels)
            {
                //render the 'render models'(great name valve)
                renderModels.Draw(CurrentViewProjMatrix(eye));
            }

            foreach (IGLDrawable drawable in drawables)
            {
                drawable.Draw(CurrentViewProjMatrix(eye));
            }            
        }

        private Matrix4 CurrentViewProjMatrix(EVREye eye)
        {
            switch (eye)
            {
                case EVREye.Eye_Left:
                    //return leftEyeProj * leftEyeView * hmdViewMatrix;
                    return hmdViewMatrix * leftEyeView * leftEyeProj;
                case EVREye.Eye_Right:
                    //return rightEyeProj * rightEyeView * hmdViewMatrix;
                    return hmdViewMatrix * rightEyeView * rightEyeProj;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eye));
            }
        }


    }
}
