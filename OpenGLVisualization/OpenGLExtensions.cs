using LabBox.OpenGLVisualization.ViewModel;
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
        public static OpenGLVertex[] Vertices(this IEnumerable<PrimitiveTriangle> tris) => tris.SelectMany(t => t.Vertices()).ToArray();
        public static OpenGLVertex[] Vertices(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Vertices()).ToArray();

        /// <summary>
        /// Row major order.
        /// </summary>
        public static float[] Flatten(this Matrix4 m) =>
            new[] { m.M11, m.M12, m.M13, m.M14,
                    m.M21, m.M22, m.M23, m.M24,
                    m.M31, m.M32, m.M33, m.M34,
                    m.M41, m.M42, m.M43, m.M44};

        public static OpenGLLightSource ToGLLight(this ILightSource light)
        {
            return new OpenGLLightSource()
            {
                AmbientIntensity = light.AmbientIntensity,
                AttenuationCoef = light.AttenuationCoef,
                DiffusePower = light.DiffusePower,
                ConeAngle = light.ConeAngle,
                Pos = light.LightType == LightType.Directional ? new Vector4(-light.LightDir.ToGLVector3(), 0.0f) : new Vector4(light.Pos.ToGLVector3(), 1.0f),
                Color = light.LightColor.ToGLVector3(),
                ConeDir = light.LightDir.ToGLVector3(),
                ShadowMapID = -1,
                FrameBufferID = -1
            };
        }
    }  
}
