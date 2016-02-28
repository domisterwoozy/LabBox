using LabBox.Visualization.Universe.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization
{
    public struct OpenGLVertex
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector3 Normal;

        public static readonly int PositionOffset = 0;
        public static readonly int ColorOffset = Vector3.SizeInBytes;
        public static readonly int NormalOffset = ColorOffset + Vector4.SizeInBytes;
        public static readonly int Stride = NormalOffset + Vector3.SizeInBytes;
    }

    public struct OpenGLLightSource
    {
        public float AmbientIntensity { get; set; }
        public float AttenuationCoef { get; set; }
        public float DiffusePower { get; set; }
        public Vector4 Pos { get; set; }
        public Vector3 Color { get; set; }
        public float ConeAngle { get; set; }
        public Vector3 ConeDir { get; set; }
    }

    public static class OpenGLExtensions
    {
        public static Vector3 ToGLVector3(this Math3D.Vector3 v) => new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        public static Quaternion ToGLQuaternion(this Math3D.Quaternion q) => new Quaternion(q.V.ToGLVector3(), (float)q.S);

        public static float ToColorComponent(this byte b) => (float)b / byte.MaxValue;
        public static Vector3 ToGLVector3(this Color c) => new Vector3(c.R.ToColorComponent(), c.G.ToColorComponent(), c.B.ToColorComponent());
        public static Vector4 ToGLVector4(this Color c) => new Vector4(c.R.ToColorComponent(), c.G.ToColorComponent(), c.B.ToColorComponent(), c.A.ToColorComponent());
        public static Vector4[] Colors(this PrimitiveTriangle t) => new[] { t.A.Color.ToGLVector4(), t.B.Color.ToGLVector4(), t.C.Color.ToGLVector4() };
        public static Vector4[] Colors(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Colors()).ToArray();

        public static Vector3[] Positions(this PrimitiveTriangle t) => new[] { t.A.Pos.ToGLVector3(), t.B.Pos.ToGLVector3(), t.C.Pos.ToGLVector3() };
        public static Vector3[] Positions(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Positions()).ToArray();

        public static OpenGLVertex Vertex(this Vertex v) => new OpenGLVertex() { Position = v.Pos.ToGLVector3(), Color = v.Color.ToGLVector4(), Normal = v.Normal.ToGLVector3() };
        public static OpenGLVertex[] Vertices(this PrimitiveTriangle t) => new[] { t.A.Vertex(), t.B.Vertex(), t.C.Vertex() };
        public static OpenGLVertex[] Vertices(this PrimitiveTriangle[] tris) => tris.SelectMany(t => t.Vertices()).ToArray();
        public static OpenGLVertex[] Vertices(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Vertices()).ToArray();

        public static OpenGLLightSource ToGLLight(this LightSource light)
        {
            return new OpenGLLightSource()
            {
                AmbientIntensity = light.AmbientIntensity,
                AttenuationCoef = light.AttenuationCoef,
                DiffusePower = light.DiffusePower,
                ConeAngle = light.ConeAngle,
                Pos = light.LightType == LightType.Directional ? new Vector4(-light.LightDir.ToGLVector3(), 0.0f) : new Vector4(light.Pos.ToGLVector3(), 1.0f),
                Color = light.LightColor.ToGLVector3(),
                ConeDir = light.LightDir.ToGLVector3()
            };
        }
    }

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

        public static void UseTexture(int textureID)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
        }

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
