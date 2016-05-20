using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace HelloVR.Shaders
{
    public sealed class RenderModelProgram : OpenGLProgram
    {
        public RenderModelProgram() : base("rendermodelvs.glsl", "rendermodelfs.glsl") { }

        public void SetMatrix(Matrix4 mat)
        {
            GL.UniformMatrix4(GetUniformID("matrix"), false, ref mat);
        }


        public override void LoadBuffer(int vertexBufferID)
        {
            throw new NotImplementedException();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
            //GL.VertexAttribPointer(GetAttribID("position"), 4, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.PositionOffset);
            //GL.VertexAttribPointer(GetAttribID("normal"), 3, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.NormalOffset);
            //GL.VertexAttribPointer(GetAttribID("texCoordIn"), 2, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.TexCoordOffset);
        }
    }
}
