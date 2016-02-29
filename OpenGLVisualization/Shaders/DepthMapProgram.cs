using LabBox.OpenGLVisualization.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization.Shaders
{
    public class DepthMapProgram : OpenGLProgram
    {
        public DepthMapProgram() : base("depthvs.glsl", "depthfs.glsl") { }

        public void SetMVP(Matrix4 mvp)
        {
            GL.UniformMatrix4(GetUniformID("MVP"), false, ref mvp);
        }

        /// <summary>
        /// Only position is required. No normals or colors.
        /// </summary>
        public override void LoadBuffer(int vertexBufferID)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
            GL.VertexAttribPointer(GetAttribID("vertexPos"), 3, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.PositionOffset);
        }
    }
}
