using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.IO;


namespace HelloVR
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
        
        public static int CreateVertexArrayObject()
        {
            int vaoID;
            GL.GenVertexArrays(1, out vaoID);
            return vaoID;
        }

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
    }
}
