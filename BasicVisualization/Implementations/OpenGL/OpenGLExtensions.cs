using BasicVisualization.Universe.ViewModel;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization.Implementations.OpenGL
{
    public static class OpenGLExtensions
    {
        public static Vector3 ToGLVector3(this Math3D.Vector3 v) => new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        public static Quaternion ToGLQuaternion(this Math3D.Quaternion q) => new Quaternion(q.V.ToGLVector3(), (float)q.S);

        public static float ToColorComponent(this byte b) => (float)b / byte.MaxValue;
        public static Vector3 ToGLVector3(this Color c) => new Vector3(c.R.ToColorComponent(), c.G.ToColorComponent(), c.B.ToColorComponent());
        public static Vector3[] Colors(this PrimitiveTriangle t) => new[] { t.A.Color.ToGLVector3(), t.B.Color.ToGLVector3(), t.C.Color.ToGLVector3() };
        public static Vector3[] Colors(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Colors()).ToArray();

        public static Vector3[] Vertices(this PrimitiveTriangle t) => new[] { t.A.Pos.ToGLVector3(), t.B.Pos.ToGLVector3(), t.C.Pos.ToGLVector3() };
        public static Vector3[] Vertices(this IGraphicalBody b) => b.Triangles.SelectMany(t => t.Vertices()).ToArray();        
    }
}
