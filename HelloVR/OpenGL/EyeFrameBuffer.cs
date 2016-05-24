using System;
using OpenTK.Graphics.OpenGL4;

namespace HelloVR.OpenGL
{
    public class EyeFrameBuffer
    {
        public int DepthBufferID { get; }
        public int RenderTextureID { get; }
        public int RenderFrameBufferID { get; }
        public int ResolveTextureID { get; }
        public int ResolveFrameBufferID { get; }

        private EyeFrameBuffer(int depthBufferID, int renderTextureID, int frameBuffer, int resolveTextureID, int resolveFrameBufferID)
        {
            DepthBufferID = depthBufferID;
            RenderTextureID = renderTextureID;
            RenderFrameBufferID = frameBuffer;
            ResolveTextureID = resolveTextureID;
            ResolveFrameBufferID = resolveFrameBufferID;
        }

        public static EyeFrameBuffer Create(int width, int height)
        {
            int frameBuffer = OpenGLUtil.CreateFrameBuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);

            int depthBuffer = OpenGLUtil.CreateRenderBuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);

            int renderTexture = OpenGLUtil.CreateTexture();
            GL.BindTexture(TextureTarget.Texture2DMultisample, renderTexture);
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Rgba8, width, height, true);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2DMultisample, renderTexture, 0);

            int resolveFrameBuffer = OpenGLUtil.CreateFrameBuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, resolveFrameBuffer);

            int resolveTexture = OpenGLUtil.CreateTexture();
            GL.BindTexture(TextureTarget.Texture2D, resolveTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, resolveTexture, 0);

            OpenGLUtil.CheckInvalidFrameBuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0); // go back to default frame buffer

            return new EyeFrameBuffer(depthBuffer, renderTexture, frameBuffer, resolveTexture, resolveFrameBuffer);
        }
    }
}
