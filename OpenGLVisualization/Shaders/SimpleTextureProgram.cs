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
    public class SimpleTextureProgram : OpenGLProgram
    {
        public SimpleTextureProgram() : base("passvs.glsl", "texturefs.glsl") { }

        /// <summary>
        /// Only position is required. No normals or colors.
        /// </summary>
        public override void LoadBuffer(int vertexBufferID)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
            GL.VertexAttribPointer(GetAttribID("vertexPos"), 3, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.PositionOffset);
        }

        public void SetTexture(int textureID)
        {
            // bind the texture to 'Texture Unit 0'
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.Uniform1(GetUniformID("texture"), 0); // puts the texture currently in 'Texture Unit 0' into the uniform for our texture
        }
    }
}
