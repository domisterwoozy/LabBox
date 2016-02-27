using LabBox.Visualization.Universe.ViewModel;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static readonly int PositionOffset = 0;
        public static readonly int ColorOffset = Vector3.SizeInBytes;
        public static readonly int Stride = Vector3.SizeInBytes + Vector4.SizeInBytes;
    }

    public static class OpenGLExtensions
    {
        public static Vector3 ToGLVector3(this Math3D.Vector3 v) => new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        public static Quaternion ToGLQuaternion(this Math3D.Quaternion q) => new Quaternion(q.V.ToGLVector3(), (float)q.S);

        public static float ToColorComponent(this byte b) => (float)b / byte.MaxValue;
        public static Vector4 ToGLVector4(this Color c) => new Vector4(c.R.ToColorComponent(), c.G.ToColorComponent(), c.B.ToColorComponent(), c.A.ToColorComponent());
        public static Vector4[] Colors(this PrimitiveTriangle t) => new[] { t.A.Color.ToGLVector4(), t.B.Color.ToGLVector4(), t.C.Color.ToGLVector4() };
        public static Vector4[] Colors(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Colors()).ToArray();

        public static Vector3[] Positions(this PrimitiveTriangle t) => new[] { t.A.Pos.ToGLVector3(), t.B.Pos.ToGLVector3(), t.C.Pos.ToGLVector3() };
        public static Vector3[] Positions(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Positions()).ToArray();

        public static OpenGLVertex[] Vertices(this PrimitiveTriangle t) => t.Positions().Zip(t.Colors(), (p, c) => new OpenGLVertex() { Position = p, Color = c }).ToArray();
        public static OpenGLVertex[] Vertices(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Vertices()).ToArray();
    }
}
