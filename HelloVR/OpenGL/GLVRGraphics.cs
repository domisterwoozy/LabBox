using OpenTK;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using Valve.VR;

namespace HelloVR.OpenGL
{
    public class GLVRGraphics : IVRGraphics
    {
        // hmd readonly fields
        public readonly bool showRenderModels;

        private readonly int renderWidth;
        private readonly int renderHeight;
        private readonly EyeFrameBuffer leftEyeFrameBuffer;
        private readonly EyeFrameBuffer rightEyeFrameBuffer;
        private readonly GLRenderModels renderModels;

        public GLVRGraphics(OpenVRScene scene, bool showRenderModels = true)
        {
            this.showRenderModels = showRenderModels;

            var dim = scene.GetRenderTarget();
            renderWidth = dim.Item1;
            renderHeight = dim.Item2;
            leftEyeFrameBuffer = EyeFrameBuffer.Create(renderWidth, renderHeight);
            rightEyeFrameBuffer = EyeFrameBuffer.Create(renderWidth, renderHeight);

            if (showRenderModels) renderModels = GLRenderModels.Create(scene);
        }     

        public EyeTextures RenderToTextures(Matrix4 leftViewProj, Matrix4 rightViewProj, Vector3 leftEyePos, Vector3 rightEyePos, params IVRDrawable[] drawables)
        {
            // left eye
            GL.Enable(EnableCap.Multisample);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, leftEyeFrameBuffer.RenderFrameBufferID);
            GL.Viewport(0, 0, renderWidth, renderHeight);
            RenderEye(leftViewProj, leftEyePos, drawables);
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
            RenderEye(rightViewProj, rightEyePos, drawables);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.Multisample);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, rightEyeFrameBuffer.RenderFrameBufferID);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, rightEyeFrameBuffer.ResolveFrameBufferID);
            GL.BlitFramebuffer(0, 0, renderWidth, renderHeight, 0, 0, renderWidth, renderHeight, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            // convert opengl textures to openvr textures  
            Texture_t leftEyeTexture = new Texture_t { handle = new IntPtr(leftEyeFrameBuffer.ResolveFrameBufferID), eType = EGraphicsAPIConvention.API_OpenGL, eColorSpace = EColorSpace.Gamma };
            Texture_t rightEyeTexture = new Texture_t { handle = new IntPtr(rightEyeFrameBuffer.ResolveFrameBufferID), eType = EGraphicsAPIConvention.API_OpenGL, eColorSpace = EColorSpace.Gamma };

            return new EyeTextures(leftEyeTexture, rightEyeTexture);
        }

        private void RenderEye(Matrix4 viewProj, Vector3 eyePos, IEnumerable<IVRDrawable> drawables)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //render the 'render models'(great name valve)
            if (showRenderModels) renderModels.Draw(viewProj, eyePos);

            foreach (IVRDrawable drawable in drawables) drawable.Draw(viewProj, eyePos);
        }
    }
}
