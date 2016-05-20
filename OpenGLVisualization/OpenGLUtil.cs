using LabBox.OpenGLVisualization.ViewModel;
using LabBox.Visualization;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization
{
    public static class OpenGLUtil
    {
        /// <summary>
        /// Loads a shader into the OpenGL pipeline.
        /// It loads the shader from a file, compiles the shader, attaches the shader to a program, and then returns the shaders opengl address.
        /// </summary>
        public static int LoadShader(string filename, ShaderType shaderType, int programAddr)
        {
            int shaderAddr = GL.CreateShader(shaderType); // create the new address
            using (var sr = new StreamReader(filename)) // apply the file source code to that address
            {
                GL.ShaderSource(shaderAddr, sr.ReadToEnd());
            }
            GL.CompileShader(shaderAddr); // compile the source code
            GL.AttachShader(programAddr, shaderAddr); // attach the shader to the program
            Console.WriteLine(GL.GetShaderInfoLog(shaderAddr));
            return shaderAddr;
        }
        

        /// <summary>
        /// Generates a single Vertex Array Object and returns the OpenGL address.
        /// </summary>
        public static int CreateVertexArrayObject()
        {
            int vaoID;
            GL.GenVertexArrays(1, out vaoID);
            return vaoID;
        }

        /// <summary>
        /// Generates a single Buffer Object and returns the OpenGL address.
        /// </summary>
        public static int CreateBufferObject()
        {
            int bufferID;
            GL.GenBuffers(1, out bufferID);
            return bufferID;
        }

        public static int CreateTexture()
        {
            int textureID;
            GL.GenTextures(1, out textureID);
            return textureID;
        }

        public static int CreateImageTexture(Bitmap image)
        {
            int textureID = CreateTexture();
            UseTexture2D(textureID);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.GetBits());
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0); // unbind texture
            return textureID;
        }        

        public static int CreateDepthTexture(int frameBufferID, int size)
        {
            UseFrameBuffer(frameBufferID);
            int textureID = CreateTexture();
            UseTexture2D(textureID);
            // specify the texture format and parameters
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent16, size, size, 0, PixelFormat.DepthComponent, PixelType.Float, (IntPtr)0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareFunc, (int)DepthFunction.Lequal);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRToTexture);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, textureID, 0);
            GL.DrawBuffer(DrawBufferMode.None); // no color buffer is drawn to
            CheckInvalidFrameBuffer();
            return textureID;
        }

        public static void DisableTextureCompare(int textureID)
        {
            UseTexture2D(textureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.None);
        }

        public static void EnableTextureCompare(int textureID)
        {
            UseTexture2D(textureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRToTexture);
        }

        public static void CheckInvalidFrameBuffer()
        {
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new InvalidOperationException("Frame buffer is in an invalid state: " + GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
            }
        }

        public static int CreateFrameBuffer()
        {
            int frameBufferID;
            GL.GenFramebuffers(1, out frameBufferID);
            return frameBufferID;
        }

        public static int CreateRenderBuffer()
        {
            int renderBufferID;
            GL.GenRenderbuffers(1, out renderBufferID);
            return renderBufferID;
        }

        public static void UseTexture2D(int textureID)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
        }

        public static void UseCubemap(int textureID)
        {
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);
        }

        /// <summary>
        /// Use a custom framebuffer instead of the default one (the screen).
        /// Passing in a 0 here will go back to the default (screen).
        /// </summary>
        public static void UseFrameBuffer(int frameBufferID)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBufferID);
        }

        /// <summary>
        /// Sets the specified Vertex Array Object as the current one.
        /// </summary>
        public static void UseVertexArrayObject(int vaoID)
        {
            GL.BindVertexArray(vaoID);
        }


        public static void PopulateBuffer(int bufferID, OpenGLVertex[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID); //sets the specified Buffer Object as the current one.
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * OpenGLVertex.Stride), data, BufferUsageHint.StaticDraw);
        }
    }
}
